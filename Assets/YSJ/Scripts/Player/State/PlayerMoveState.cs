using PVS;

using UnityEngine;

namespace ProjectVS.Player.State
{
    public class PlayerMoveState : PlayerState
    {
        public PlayerMoveState(PlayerController controller) : base(controller) { }

        private float m_moveSpeed;

        public override void Enter()
        {
            Debug.Log("Entered Move State");
            m_moveSpeed = _controller.Status.CurrentSpd;
        }

        public override void Update()
        {
            if (_controller.MoveDirection != Vector3.zero)
                _controller.transform.Translate(m_moveSpeed * Time.deltaTime * _controller.MoveDirection);
            else
                _controller.ChangeState(PlayerStateType.Idle);
        }
    }
}
