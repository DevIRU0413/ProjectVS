using System.Collections.Generic;

using ProjectVS.Player.State;

using PVS;
using ProjectVS.Util;

using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectVS.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PlayerConfig))]
    public class PlayerController : MonoBehaviour
    {
        // status
        private PlayerConfig m_config;
        public PlayerStatus Status { get; private set; }

        // state
        private Dictionary<PlayerStateType, PlayerState> m_states = new();

        [field: Header("State")]
        public PlayerStateType CurrentStateType { get; private set; }
        public PlayerState m_currentState;

        // Player Value
        [Header("Information")]
        [field: SerializeField] private int m_level = 1;
        [field: SerializeField] private bool m_isInputValue = true;

        // input value
        public Vector3 MoveDirection { get; private set; }
        public Vector2 ActionDirection { get; private set; }


        private void Awake() => Init();
        private void Update()
        {
            if (Status.CurrentHp <= 0 && CurrentStateType != PlayerStateType.Death)
                ChangeState(PlayerStateType.Death, true);
            m_currentState?.Update();
        }

        private void Init()
        {
            // 데이터
            m_config = GetComponent<PlayerConfig>();
            Status = new PlayerStatus(m_config.Hp, m_config.ATK, m_config.SPD, m_config.AtkRange);

            // 리지드바디 세팅
            var rig = gameObject.GetOrAddComponent<Rigidbody2D>();
            rig.gravityScale = 0.0f;
            rig.freezeRotation = true;

            // 상태 추가
            m_states.Add(PlayerStateType.Idle, new PlayerIdleState(this));
            m_states.Add(PlayerStateType.Move, new PlayerMoveState(this));
            m_states.Add(PlayerStateType.Death, new PlayerDeathState(this));

            // 상태 변경
            if (m_config == null)
                ChangeState(PlayerStateType.Death);
            else
                ChangeState(PlayerStateType.Idle);
        }
        public void ChangeState(PlayerStateType stateType, bool isForceChange = false)
        {
            if (CurrentStateType == PlayerStateType.Death && !isForceChange) return;

            CurrentStateType = stateType;

            m_currentState?.Exit();
            m_currentState = m_states[CurrentStateType];
            m_currentState.Enter();
        }

        // --- New Input System 에서 Send Messages 처리용 ---
        public void OnMove(InputValue value)
        {
            if (!m_isInputValue) return;

            Vector2 input = value.Get<Vector2>();
            MoveDirection = new Vector3(input.x, input.y, 0f);
            MoveDirection.Normalize();
        }
        public void OnMousePosition(InputValue value)
        {
            if (!m_isInputValue) return;

            Vector2 input = value.Get<Vector2>();
            ActionDirection = input;
        }
    }
}
