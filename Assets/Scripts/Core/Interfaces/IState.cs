namespace ProjectVS.Core.FSM
{
    public interface IState<TStateType> where TStateType : struct, System.Enum
    {
        TStateType StateType { get; }

        void Enter();
        void Tick(float deltaTime);
        void Exit();

        bool CanTransitionTo(TStateType targetState);
    }
}
