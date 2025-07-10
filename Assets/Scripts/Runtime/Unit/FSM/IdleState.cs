namespace ProjectVS.Runtime.Unit.FSM
{
    public class IdleState : UnitState
    {
        public IdleState(BaseUnitController unit) : base(unit) { }

        public override UnitStateType StateType => UnitStateType.Idle;

        public override void Enter()
        {
            unit.PlayAnimation("Idle");
        }

        public override void Tick(float deltaTime)
        {
            // 입력 또는 AI 등으로 이동 조건 발생 시
            if (ShouldMove())
            {
                unit.FSM.ChangeState(UnitStateType.Move);
            }
        }

        public override void Exit()
        {
            // 상태 종료 시 처리할 것 있으면 여기에
        }

        private bool ShouldMove()
        {
            // 플레이어면 입력, AI면 경로, 타겟 등 판단
            // 지금은 임시로 false
            return false;
        }
    }
}
