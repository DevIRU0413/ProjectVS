using ProjectVS;
using ProjectVS.Data;
using ProjectVS.Monster;
using ProjectVS.UI.Presenter;
using ProjectVS.Unit.Player;

namespace ProjectVS.Manager
{
    public class StageContext
    {
        // 필드
        public PlayerConfig Player { get; set; }
        public TimerPresenter Timer { get; set; }
        public MonsterSpawnController Spawner { get; set; }
        public MonsterController BossController { get; set; }

        public StageDataSO StageData { get; set; }
        public StageFlowState FlowState { get; set; } = StageFlowState.None;
        public StageResult StageResult { get; set; } = StageResult.None;

        /// <summary>
        /// 승리 조건 확인
        /// </summary>
        public bool IsWinConditionMet()
        {
            bool isTimeOver = Timer?.IsTimeOver() ?? false;

            bool isBossDead = StageData.BossPrefab == null
            || (BossController != null && BossController.IsDeath);

            return isTimeOver && isBossDead;
        }
    }
}
