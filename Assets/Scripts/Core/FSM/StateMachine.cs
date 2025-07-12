using System.Collections.Generic;

namespace ProjectVS.Core.FSM
{
    public class StateMachine<TStateType, TState>
        where TStateType : struct, System.Enum
        where TState : IState<TStateType>
    {
        private readonly Dictionary<TStateType, TState> _states = new();
        private TState _current;

        public TState Current => _current;
        public TStateType? CurrentStateType => _current?.StateType;

        public void RegisterState(TState state)
        {
            if (state == null) return;
            _states[state.StateType] = state;
        }

        public void ChangeState(TStateType targetStateType)
        {
            if (_current != null && _current.StateType.Equals(targetStateType))
                return;

            if (!_states.TryGetValue(targetStateType, out var nextState))
                return;

            if (_current != null && !_current.CanTransitionTo(targetStateType))
                return;

            _current?.Exit();
            _current = nextState;
            _current.Enter();
        }

        public void Tick(float deltaTime)
        {
            _current?.Tick(deltaTime);
        }

        public void Clear()
        {
            _states.Clear();
            _current = default;
        }
    }
}
