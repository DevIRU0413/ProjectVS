using ProjectVS.Interface;

namespace ProjectVS.Manager.Stage
{
    public class ExitState : IStageState
    {
        private readonly StageContext _ctx;

        public ExitState(StageFlowMachine fsm, StageContext ctx)
        {
            _ctx = ctx;
        }

        public void Enter()
        {
            UnityEngine.Debug.Log($"[Stage] 종료 상태 진입. 결과: {_ctx.StageResult}");
            // 결과창 UI 띄우기 등 처리
        }

        public void Update() { }

        public void Exit() { }
    }

}
