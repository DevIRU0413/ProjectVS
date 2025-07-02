using ProjectVS.Interface;

namespace ProjectVS.Manager.Stage
{
    public class PauseState : IStageState
    {
        public PauseState(StageFlowMachine fsm, StageContext ctx) { }

        public void Enter() => UnityEngine.Debug.Log("[Stage] 일시정지");

        public void Update() { }

        public void Exit() { }
    }

}
