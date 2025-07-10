using UnityEngine;

namespace ProjectVS.Runtime.Unit.FSM
{
    public class DeadState : UnitState
    {
        public DeadState(BaseUnitController unit) : base(unit) { }

        public override UnitStateType StateType => UnitStateType.Dead;

        public override void Enter()
        {
            unit.PlayAnimation("Run");
        }

        public override void Tick(float deltaTime)
        {
        }

        public override void Exit()
        {
            // 이동 상태 종료 시
        }
    }
}
