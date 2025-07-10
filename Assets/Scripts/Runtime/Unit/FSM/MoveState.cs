using UnityEngine;

namespace ProjectVS.Runtime.Unit.FSM
{
    public class MoveState : UnitState
    {
        public MoveState(BaseUnitController unit) : base(unit) { }

        public override UnitStateType StateType => UnitStateType.Move;

        public override void Enter()
        {
            unit.PlayAnimation("Run");
        }

        public override void Tick(float deltaTime)
        {
            // 컨트롤러 쪽에서 모듈로 처리할 예정
            // Vector3 inputDir = GetInputDirection();

            /*if (inputDir == Vector3.zero)
            {
                unit.FSM.ChangeState(UnitStateType.Idle);
                return;
            }*/

            // 이동도 모듈만들어서 처리할 예정
            //unit.Move(inputDir);
        }

        public override void Exit()
        {
            // 이동 상태 종료 시
        }
    }
}
