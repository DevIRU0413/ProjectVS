using System;
using System.Collections.Generic;

using ProjectVS.Monster.Data;
using ProjectVS.Monster.State;
using ProjectVS.Phase;
using ProjectVS.Player;

using PVS;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Monster
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(MonsterConfig))]
    [RequireComponent(typeof(MonsterPhaseController))]
    public class MonsterController : MonoBehaviour
    {
        [field: SerializeField]
        private Animator _animator;

        // 상태
        [field: SerializeField, Header("State")]
        public MonsterStateType CurrentStateType { get; private set; } = MonsterStateType.None;
        [SerializeField] private float _stopMoveRange = 0.1f;

        public bool IsStateLock { get; private set; } = false;

        public bool IsDeath => CurrentStateType == MonsterStateType.Death;
        public bool IsMove => Target != null && MoveDirection != Vector3.zero && MoveDirection.magnitude > _stopMoveRange;
        public bool IsWin => Target == null;

        // 대상
        [field: SerializeField, Header("Target")]
        public PlayerConfig Target { get; private set; }
        public Vector3 MoveDirection { get; private set; }

        // 몬스터 정보
        [field: SerializeField]
        public MonsterStatus Status { get; private set; }

        public Action OnHit;

        private MonsterConfig _config;
        private Dictionary<MonsterStateType, MonsterState> _states = new();
        private MonsterState _currentState;

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            // 죽었을 때
            if (Status.CurrentHp <= 0 && CurrentStateType != MonsterStateType.Death)
            {
                UnLockChangeState();
                ChangeState(MonsterStateType.Death, true);
            }

            GetMoveDirection();
            _currentState?.Update();
        }

        private void Init()
        {
            // 데이터
            _config = GetComponent<MonsterConfig>();
            Status = new MonsterStatus(_config.Hp, _config.ATK, _config.SPD, _config.AtkRange, _config.ExpPoint);

            // 리지드바디 세팅
            var rig =gameObject.GetOrAddComponent<Rigidbody2D>();
            rig.gravityScale = 0.0f;
            rig.freezeRotation = true;

            // 상태
            _states.Add(MonsterStateType.Idle, new MonsterIdleState(this, _animator));
            _states.Add(MonsterStateType.Move, new MonsterMoveState(this, _animator));
            _states.Add(MonsterStateType.Win, new MonsterWinState(this, _animator));
            _states.Add(MonsterStateType.Death, new MonsterDeathState(this, _animator));

            // 타겟 세팅
            var playerGo = GameObject.FindGameObjectWithTag("Player");
            Target = playerGo?.GetComponentInParent<PlayerConfig>();

            // 초기 상태 세팅
            if (_config == null)
                ChangeState(MonsterStateType.Death, true);
            else if (Target == null)
                ChangeState(MonsterStateType.Win);
            else
                ChangeState(MonsterStateType.Idle);

            // 상태 락 관련 세팅
            IsStateLock = false;
        }

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

        private void GetMoveDirection()
        {
            MoveDirection = (Target.transform.position - transform.position);
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            // 방향
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + MoveDirection.normalized * 3.0f);
                
            Gizmos.DrawWireSphere(transform.position, _stopMoveRange);
        }
    }
}
