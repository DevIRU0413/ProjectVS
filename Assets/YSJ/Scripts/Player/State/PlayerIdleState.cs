using PVS;

using UnityEngine;

namespace ProjectVS.Player.State
{
    public class PlayerIdleState : PlayerState
    {
        public PlayerIdleState(PlayerController controller) : base(controller) { }

        public override void Enter()
        {
            Debug.Log("Entered Idle State");
            // 애니메이션 시작 처리
        }

        public override void Exit()
        {
            Debug.Log("Exited Idle State");
        }

        public override void Update()
        {
            if (_controller.MoveDirection != Vector3.zero)
            {
                _controller.ChangeState(PlayerStateType.Move);
            }
        }
    }
}
