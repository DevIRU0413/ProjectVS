using System.Collections.Generic;

using ProjectVS.Interface;

using UnityEditor;

using UnityEngine.Playables;

namespace ProjectVS.Manager.Stage
{
    public class StageFlowMachine
    {
        private IStageState _currentState;
        private readonly Dictionary<StageFlowState, IStageState> _states;
        private readonly StageContext _context;

        public StageFlowMachine(StageContext context)
        {
            _context = context;
            _states = new Dictionary<StageFlowState, IStageState>
        {
            { StageFlowState.Enter, new EnterState(this, _context) },
            { StageFlowState.Play, new PlayState(this, _context) },
            { StageFlowState.Pause, new PauseState(this, _context) },
            { StageFlowState.Exit, new ExitState(this, _context) }
        };
        }

        public void Enter() => ChangeState(StageFlowState.Enter);

        public void ChangeState(StageFlowState newState)
        {
            _currentState?.Exit();
            _currentState = _states[newState];
            _currentState.Enter();
        }

        public void Update() => _currentState?.Update();

        public void OnGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.Pause)
                ChangeState(StageFlowState.Pause);
            else if (gameState == GameState.Play && _currentState is PauseState)
                ChangeState(StageFlowState.Play);
        }
    }

}
