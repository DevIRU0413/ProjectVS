using UnityEngine;

namespace ProjectVS.Runtime.Unit.FSM
{
    public class MoveState : UnitState
    {
        public MoveState(UnitController unit) : base(unit) { }

        public override UnitStateType StateType => UnitStateType.Move;

        public override void Enter()
        {
            unit.PlayAnimation("Run");
        }

        public override void Tick(float deltaTime)
        {
            Vector3 inputDir = GetInputDirection();
            if (inputDir == Vector3.zero)
            {
                unit.StateMachine.ChangeState(UnitStateType.Idle);
                return;
            }

            unit.Move(inputDir);
        }

        public override void Exit()
        {
            // 이동 상태 종료 시
        }

        private Vector3 GetInputDirection()
        {
            // 예: WASD 입력 받기
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            return new Vector3(h, 0f, v);
        }
    }
}
