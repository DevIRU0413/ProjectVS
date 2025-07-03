using System;
using System.Collections.Generic;

using ProjectVS.Interface;
using ProjectVS.Monster.Data;
using ProjectVS.Monster.State;
using ProjectVS.Phase;
using ProjectVS.Unit.Monster;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Monster
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(UnitStatsConfig))]
    [RequireComponent(typeof(MonsterPhaseController))]
    public class MonsterController : MonoBehaviour, IDamageable, IPoolable
    {
        // FSM 상태 관리
        private Dictionary<MonsterStateType, MonsterState> _states = new();
        private MonsterState _currentState;

        [field: SerializeField] public int MonsterID { get; private set; }

        [field: SerializeField] public MonsterStateType CurrentStateType { get; private set; } = MonsterStateType.None;
        [field: SerializeField] public bool IsStateLock { get; private set; } = false;

        [field: SerializeField] public GameObject Target { get; private set; }

        // 상태 판단 프로퍼티
        public bool IsDeath => CurrentStateType == MonsterStateType.Death;
        public bool IsMove => MoveDirection != Vector3.zero && MoveDirection.magnitude > _stopMoveRange;
        public bool IsWin => false;

        public MonsterAnimationPlayer Anim { get; private set; }
        public MonsterStats Stats { get; private set; }
        public Vector3 MoveDirection { get; private set; }

        public Action OnHit { get; set; }
        public Action OnDeath;

        // 이동 관련
        [Header("Move State")]
        [SerializeField]
        private float _stopMoveRange = 0.1f;
        private bool _isMovementDelegated = false;

        // 사망 관련
        [field: Header("Death State")]
        [field: SerializeField, Min(0)] public float DespawnDelay { get; private set; } = 1.0f;
        

        private void Awake() => Init();
        private void Update()
        {
            // 체력이 없는데, 죽지 않았을 때
            if (Stats.CurrentHp <= 0 && CurrentStateType != MonsterStateType.Death)
            {
                UnLockChangeState();
                ChangeState(MonsterStateType.Death, true);
            }

            // 이동 권한 위임이 안일어났을 때
            if (!_isMovementDelegated && Target != null)
                SetMoveDirection(Target.transform.position);

            _currentState?.Update();
        }
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            // 방향
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + MoveDirection.normalized * 3.0f);

            Gizmos.DrawWireSphere(transform.position, _stopMoveRange);
        }

        // 기본 데이터 세팅
        public void Init()
        {
            // 리지드바디 세팅
            var rig = gameObject.GetOrAddComponent<Rigidbody2D>();
            rig.gravityScale = 0.0f;
            rig.freezeRotation = true;

            // 애니메이션 플레이어
            var anim = GetComponentInChildren<Animator>();
            Anim = new MonsterAnimationPlayer(anim, this);

            // 상태 추가
            _states.Add(MonsterStateType.Idle, new MonsterIdleState(this, Anim.Animator));
            _states.Add(MonsterStateType.Move, new MonsterMoveState(this, Anim.Animator));
            _states.Add(MonsterStateType.Win, new MonsterWinState(this, Anim.Animator));
            _states.Add(MonsterStateType.Death, new MonsterDeathState(this, Anim.Animator));
        }
        public void SetTarget(GameObject target) => Target = target;

        // 상태 전환 관련
        public void ChangeState(MonsterStateType stateType, bool isForceChange = false)
        {
            if (CurrentStateType == MonsterStateType.Death && !isForceChange) return;
            if (IsStateLock) return;
            if (CurrentStateType == stateType) return;

            CurrentStateType = stateType;

            _currentState?.Exit();
            _currentState = _states[CurrentStateType];
            _currentState.Enter();
        }
        public void LockChangeState()
        {
            IsStateLock = true;
        }
        public void UnLockChangeState()
        {
            IsStateLock = false;
        }

        // 이동 권한 위임, 해제
        public void DelegateMovementAuthority()
        {
            if (_isMovementDelegated) return;
            _isMovementDelegated = true;
        }
        public void RevokeMovementAuthority()
        {
            if (!_isMovementDelegated) return;
            _isMovementDelegated = false;
        }

        public void SetMoveDirection(Vector3 movePoint, bool onlySetMovePoint = false)
        {
            MoveDirection = (onlySetMovePoint) ? movePoint : (movePoint - transform.position);
        }

        // IDamageable
        public void TakeDamage(DamageInfo info)
        {
            Debug.Log("몬스터 데미지 테스트");
            Stats.CurrentHp -= info.Amount;
            OnHit.Invoke();
        }

        public void OnSpawned()
        {
            var config = GetComponent<UnitStatsConfig>();
            if (config != null)
                Stats = new MonsterStats(config.Hp, config.ATK, config.DFS, config.SPD, config.ATKSPD);

            // 상태 락 관련 세팅
            IsStateLock = false;

            // 초기 상태 세팅
            if (Stats == null)
                ChangeState(MonsterStateType.Death, true);
            else
            {
                if (Stats.CurrentHp > 0)
                    ChangeState(MonsterStateType.Idle);
                else
                    ChangeState(MonsterStateType.Death);
            }
        }
        public void OnDespawned() { }
    }
}
