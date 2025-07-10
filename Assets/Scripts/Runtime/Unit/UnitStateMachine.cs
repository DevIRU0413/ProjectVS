using ProjectVS.Core.FSM;
using ProjectVS.Runtime.Unit;
using ProjectVS.Runtime.Unit.FSM;

namespace ProjectVS.FSM
{
    public class UnitStateMachine : StateMachine<UnitStateType, UnitState>
    {
        public UnitStateMachine(BaseUnitController owner)
        {
            RegisterState(new IdleState(owner));
            RegisterState(new MoveState(owner));
            RegisterState(new DeadState(owner));
        }
    }
}
