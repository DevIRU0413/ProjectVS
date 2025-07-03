using ProjectVS.Interface;

using UnityEngine;

namespace ProjectVS.Manager.Stage
{
    public class PlayState : IStageState
    {
        private readonly StageFlowMachine _fsm;
        private readonly StageContext _ctx;

        public PlayState(StageFlowMachine fsm, StageContext ctx)
        {
            _fsm = fsm;
            _ctx = ctx;
        }

        public void Enter()
        {
            Debug.Log("[Stage] Play 시작");

        }
        public void Update()
        {
            if (_ctx.Player.Stats.CurrentHp <= 0)
            {
                _ctx.StageResult = StageResult.Lose;
                _fsm.ChangeState(StageFlowState.Exit);
                return;
            }

            if (_ctx.Timer != null)
                _ctx.Timer.Update(UnityEngine.Time.deltaTime);

            _ctx.Spawner?.Update();

            if (_ctx.IsWinConditionMet())
            {
                _ctx.StageResult = StageResult.Win;
                _fsm.ChangeState(StageFlowState.Exit);
            }
        }

        public void Exit() { }
    }
}
