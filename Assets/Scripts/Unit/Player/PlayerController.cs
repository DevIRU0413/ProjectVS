namespace ProjectVS.Unit.Player
{
    using System;

    using ProjectVS.Interface;

    using UnityEngine;

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerInputHandler))]
    [RequireComponent(typeof(PlayerConfig))]
    public class PlayerController : MonoBehaviour, IDamageable
    {
        private Rigidbody2D _rigid;
        private PlayerInputHandler _input;
        private PlayerConfig _config;

        private PlayerStats _stats;

        [SerializeField] private Transform _rotateTarget; // 무기 피벗 또는 본체

        public Vector2 MoveDirection => _input.MoveDirection;
        public Action OnHit { get; set; }

        private void Awake()
        {
            _rigid = GetComponent<Rigidbody2D>();
            _input = GetComponent<PlayerInputHandler>();
            _config = GetComponent<PlayerConfig>();

            if (_rotateTarget == null)
                _rotateTarget = transform;
        }

        private void Start()
        {
            _stats = _config.Stats;
            if (_stats == null)
            {
                Debug.LogError("[PlayerController] PlayerStats가 초기화되지 않았습니다.");
            }
        }

        private void Update()
        {
            Move();
            Rotate();
        }

        private void Move()
        {
            if (_stats == null) return;

            Vector2 dir = _input.MoveDirection;
            float speed = _stats.CurrentSpd;

            transform.Translate(Time.deltaTime * speed * dir);
        }

        private void Rotate()
        {
            Vector2 target = _input.MouseWorldPosition;
            Vector2 lookDir =  target - (Vector2)_rotateTarget.position;

            if (lookDir.x < 0)
                _rotateTarget.localScale = new Vector3(1, 1, 1);
            else if (lookDir.x > 0)
                _rotateTarget.localScale = new Vector3(-1, 1, 1);
        }

        // IDamageable
        public void TakeDamage(DamageInfo info)
        {
            _stats.CurrentHp -= info.Amount;
            OnHit?.Invoke();
        }
    }

}
