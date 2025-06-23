using System;
using System.Collections.Generic;

using PVS.Monster.State;
using PVS.Player;

using UnityEngine;

namespace PVS.Monster
{
    [RequireComponent(typeof(MonsterConfig))]
    [RequireComponent(typeof(MonsterPhaseController))]
    public class MonsterController : MonoBehaviour
    {
        // status
        private MonsterConfig m_config;
        [field: SerializeField] public MonsterStatus Status { get; private set; }

        // state
        private Dictionary<MonsterStateType, MonsterState> m_states = new();

        [field: Header("State")]
        public MonsterStateType CurrentStateType { get; private set; } = MonsterStateType.None;
        public MonsterState m_currentState;

        public bool IsStateLock { get; private set; } = false;
        public bool IsDeath => CurrentStateType == MonsterStateType.Death;

        public Action OnHit;

        // Monster Value
        [field: Header("Target")]
        [field: SerializeField]
        public PlayerController Target { get; private set; }
        public Vector3 MoveDirection { get; private set; }


        private void Awake() => Init();
        private void Update()
        {
            if (Target == null) return;
            if (Target.CurrentStateType == PlayerStateType.Death)
            {

            }

            GetMoveDirection();
            m_currentState?.Update();
        }

        private void Init()
        {
            m_config = GetComponent<MonsterConfig>();
            Status = new MonsterStatus(m_config.Hp, m_config.ATK, m_config.SPD, m_config.AtkRange, m_config.ExpPoint);

            // 상태 추가
            m_states.Add(MonsterStateType.Idle, new MonsterIdleState(this));
            m_states.Add(MonsterStateType.Move, new MonsterMoveState(this));
            m_states.Add(MonsterStateType.Win, new MonsterWinState(this));
            m_states.Add(MonsterStateType.Death, new MonsterDeathState(this));

            // 타겟 선정
            var playerGo = GameObject.FindGameObjectWithTag("Player");
            Target = playerGo.GetComponentInParent<PlayerController>();

            // 상태 변경
            if (m_config == null || Target == null)
                ChangeState(MonsterStateType.Death, true);
            else
                ChangeState(MonsterStateType.Idle);

            IsStateLock = false;
        }

        // 강제 변경도 락이 걸려 있으면 전환 못함.
        public void ChangeState(MonsterStateType stateType, bool isForceChange = false)
        {
            // 죽고 강제라면 또는 락 걸려 있다면, 리턴
            if (CurrentStateType == MonsterStateType.Death && !isForceChange || IsStateLock) return;

            // 전환 되려는 상태가 같은 상태라면
            if (CurrentStateType == stateType) return;

            CurrentStateType = stateType;

            m_currentState?.Exit();
            m_currentState = m_states[CurrentStateType];
            m_currentState.Enter();
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
            MoveDirection = (Target.transform.position - this.transform.position).normalized;
        }

        private void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, transform.position + MoveDirection * 3.0f);
            }
        }
    }
}
