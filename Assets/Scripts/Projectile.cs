using UnityEngine;

namespace ProjectVS
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [Header("Visual")]
        [SerializeField] private GameObject body;

        [Header("Movement Settings")]
        [SerializeField] private float speed = 10f;
        [SerializeField] private float rotSpeed = 360f;
        [SerializeField] private float damping = 0.0f;
        [SerializeField] private bool useGravity = false;

        [Header("Homing Setting")]
        [SerializeField] private Transform target;
        [SerializeField, Min(-1)] private float homingTime = -1;

        private bool isHoming = false;
        private bool isFired = false;
        private Rigidbody2D rb;
        private Vector2 direction;

        private void Start()
        {
            if (target == null) return;

            Vector3 dir = target.position - transform.position;
            Fire(dir, target);
        }

        private void OnEnable()
        {
            if (target == null) return;

            Vector3 dir = target.position - transform.position;
            Fire(dir, target);
        }

        public void Fire(Vector2 fireDirection, Transform target = null)
        {
            rb = GetComponent<Rigidbody2D>();

            this.direction = fireDirection.normalized;
            this.target = target;
            isFired = true;

            rb.gravityScale = useGravity ? 1f : 0f;
            rb.velocity = direction * speed;

            isHoming = (homingTime > 0 || homingTime == -1) && target != null;
            RotateVisual(direction);
        }

        private void FixedUpdate()
        {
            if (!isFired) return;

            isHoming = (homingTime > 0 || homingTime == -1) && target != null;
            if (isHoming)
            {
                Vector2 toTarget = (target.position - transform.position).normalized;
                Vector3 rotated = Vector3.RotateTowards(
                    (Vector3)direction,
                    (Vector3)toTarget,
                    rotSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime,
                    0f
                );

                direction = rotated;
                rb.velocity = direction * speed;
                if (homingTime != -1)
                    homingTime = Mathf.Max(0, homingTime - Time.fixedDeltaTime);
            }

            if (damping > 0f)
            {
                rb.velocity *= (1 - damping * Time.fixedDeltaTime);
            }
        }

        private void Update()
        {
            if (!isFired || body == null || !isHoming) return;
            RotateVisual(direction);
        }

        private void RotateVisual(Vector2 faceDir)
        {
            if (faceDir == Vector2.zero || body == null) return;

            float angle = Mathf.Atan2(faceDir.y, faceDir.x) * Mathf.Rad2Deg;
            Quaternion targetRot = Quaternion.Euler(0, 0, angle);

            body.transform.rotation = targetRot;
        }
    }
}
