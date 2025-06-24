using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField] private bool isHoming = false;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 5f;

    private Rigidbody2D rb;
    private Vector2 direction;
    private Transform target;
    private bool isFired = false;

    public void Fire(Vector2 fireDirection, Transform target = null)
    {
        rb = GetComponent<Rigidbody2D>();

        this.direction = fireDirection.normalized;
        this.target = target;
        isFired = true;

        rb.gravityScale = useGravity ? 1f : 0f;
        rb.velocity = direction * speed;

        RotateVisual(direction);

        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (!isFired) return;

        if (isHoming && target != null)
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
        }

        if (damping > 0f)
        {
            rb.velocity *= (1 - damping * Time.fixedDeltaTime);
        }
    }

    private void Update()
    {
        if (!isFired || body == null) return;
        RotateVisual(rb.velocity.normalized);
    }

    private void RotateVisual(Vector2 faceDir)
    {
        if (faceDir == Vector2.zero || body == null) return;

        float angle = Mathf.Atan2(faceDir.y, faceDir.x) * Mathf.Rad2Deg;
        Quaternion targetRot = Quaternion.Euler(0, 0, angle);

        body.transform.rotation = targetRot;
    }

    public static IEnumerator FireMultiPattern(
        GameObject projectilePrefab,
        Vector3 origin,
        int totalProjectiles,
        int multiShotCount,
        float multiShotSpacing,
        float spreadAngle,
        int step,
        float stepDelay,
        Transform target = null,
        bool isLook = true)
    {
        int totalGroups = totalProjectiles / multiShotCount;
        int groupsPerStep = totalGroups / step;
        float groupAngleStep = spreadAngle / groupsPerStep;
        Vector3 dir = target.position - origin;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        float baseStartAngle = angle;

        for (int s = 0; s < step; s++)
        {
            for (int g = 0; g < groupsPerStep; g++)
            {
                int globalIndex = s * groupsPerStep + g;
                float groupBaseAngle = baseStartAngle + globalIndex * groupAngleStep;
                Vector2 baseDir = Quaternion.Euler(0, 0, groupBaseAngle) * Vector2.right;

                for (int i = 0; i < multiShotCount; i++)
                {
                    float offset = (i - (multiShotCount - 1) / 2f) * multiShotSpacing;
                    Vector2 finalDir = Quaternion.Euler(0, 0, offset) * baseDir;

                    GameObject clone = GameObject.Instantiate(projectilePrefab, origin, Quaternion.identity);
                    clone.GetComponent<Projectile>()?.Fire(finalDir, target);
                }
            }

            if (s < step - 1 && stepDelay > 0f)
                yield return new WaitForSeconds(stepDelay);
        }
    }
}
