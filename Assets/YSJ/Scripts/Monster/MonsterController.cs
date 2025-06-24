using System;
using System.Collections.Generic;
using UnityEngine;
using ProjectVS.Monster.Data;
using ProjectVS.Monster.State;
using ProjectVS.Player;
using ProjectVS.Phase;
using PVS;

namespace ProjectVS.Monster
{
    [RequireComponent(typeof(MonsterConfig))]
    [RequireComponent(typeof(MonsterPhaseController))]
    public class MonsterController : MonoBehaviour
    {
        // 상태
        [field: SerializeField, Header("State")]
        public MonsterStateType CurrentStateType { get; private set; } = MonsterStateType.None;
        public bool IsStateLock { get; private set; } = false;
        public bool IsDeath => CurrentStateType == MonsterStateType.Death;

        // 대상
        [field: SerializeField, Header("Target")]
        public PlayerController Target { get; private set; }
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
            if (Target == null) return;
            if (Target.CurrentStateType == PlayerStateType.Death) return;

            GetMoveDirection();
            _currentState?.Update();
        }

        private void Init()
        {
            _config = GetComponent<MonsterConfig>();
            Status = new MonsterStatus(_config.Hp, _config.ATK, _config.SPD, _config.AtkRange, _config.ExpPoint);

            _states.Add(MonsterStateType.Idle, new MonsterIdleState(this));
            _states.Add(MonsterStateType.Move, new MonsterMoveState(this));
            _states.Add(MonsterStateType.Win, new MonsterWinState(this));
            _states.Add(MonsterStateType.Death, new MonsterDeathState(this));

            var playerGo = GameObject.FindGameObjectWithTag("Player");
            Target = playerGo.GetComponentInParent<PlayerController>();

            if (_config == null || Target == null)
                ChangeState(MonsterStateType.Death, true);
            else
                ChangeState(MonsterStateType.Idle);

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
            ChangeState(MonsterStateType.Idle, true);
            IsStateLock = true;
        }

        public void UnLockChangeState()
        {
            IsStateLock = false;
            ChangeState(MonsterStateType.Idle, true);
        }

        private void GetMoveDirection()
        {
            if (CurrentStateType == MonsterStateType.Death) return;
            MoveDirection = (Target.transform.position - transform.position).normalized;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying) return;

            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + MoveDirection * 3.0f);
        }
    }
}
