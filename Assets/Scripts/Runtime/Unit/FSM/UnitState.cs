using ProjectVS.Core.FSM;

namespace ProjectVS.Runtime.Unit.FSM
{
    public abstract class UnitState : IState<UnitStateType>
    {
        protected readonly BaseUnitController unit;

        protected UnitState(BaseUnitController unit)
        {
            this.unit = unit;
        }

        public abstract UnitStateType StateType { get; }

        public virtual void Enter() { }
        public virtual void Tick(float deltaTime) { }
        public virtual void Exit() { }

        public virtual bool CanTransitionTo(UnitStateType targetState) => true;
    }
}
