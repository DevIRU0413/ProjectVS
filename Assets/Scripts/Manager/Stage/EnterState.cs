using ProjectVS.Interface;
using ProjectVS.Manager.Stage;
using ProjectVS.Manager;
using ProjectVS;

namespace ProjectVS.Manager.Stage
{
    public class EnterState : IStageState
    {
        private readonly StageFlowMachine _fsm;
        private readonly StageContext _ctx;

        public EnterState(StageFlowMachine fsm, StageContext ctx)
        {
            _fsm = fsm;
            _ctx = ctx;
        }

        public void Enter()
        {
            UnityEngine.Debug.Log("[Stage] Enter");
            _fsm.ChangeState(GameManager.Instance.CurrentState == GameState.Play ? StageFlowState.Play : StageFlowState.Pause);
        }

        public void Update() { }

        public void Exit() { }
    }
}
