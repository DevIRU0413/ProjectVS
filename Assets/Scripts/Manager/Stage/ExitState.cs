using ProjectVS.Interface;
using ProjectVS.UIs.CutSceneEffect.CutSceneController;
using ProjectVS.Utils.UIManager;

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
            
            // 이겼을 때, 인벤토리 저장
            if (_ctx.StageResult == StageResult.Win)
            {
                Item.ItemManager.ItemManager.Instance.SendInventory();
                SceneLoader.Instance.LoadSceneAsync(SceneID.InGameScene);
            }

            if (_ctx.IsTrueEnding())
            {
                CutSceneController.Instance.PlayCutScene(CutSceneType.TrueEnding);
                return;
            }

            if (_ctx.IsNormalEnding())
            {
                CutSceneController.Instance.PlayCutScene(CutSceneType.NormalEnding);
            }

            // 결과창 UI 띄우기 등 처리
            UIManager.Instance.Show("Lose Panel");
        }

        public void Update() { }

        public void Exit()
        {
            PlayerDataManager.Instance.SavePlayerData();
        }
    }

}
