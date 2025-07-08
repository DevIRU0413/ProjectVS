using System;
using System.Collections;
using System.Collections.Generic;

using ProjectVS.Manager;

using TMPro;

using UnityEngine;


namespace ProjectVS.UIs.PanelBehaviours.LosePanelButtons
{
    public class LosePanelButtons : MonoBehaviour
    {
        [SerializeField] private Timer _timer;
        [SerializeField] private TMP_Text _playedTimeText;
        [SerializeField] private TMP_Text _totalKillCountText;
        [SerializeField] private TMP_Text _totalStageCountText;

        private void OnEnable()
        {
            RenewText();
        }

        private void RenewText()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(_timer.CurrentTime);
            _playedTimeText.text = $"클리어까지 남은 시간 {(int)timeSpan.TotalMinutes:00}분 {timeSpan.Seconds:00}초";

            _totalKillCountText.text = $"총 처치한 적의 수: {PlayerDataManager.Instance.TotalKills.ToString()}";

            _totalStageCountText.text = $"총 진입한 전투 씬의 수: {PlayerDataManager.Instance.BattleSceneCount.ToString()}";
        }

        public void OnClickMainMenu()
        {
            PlayerDataManager.Instance.DeletePlayerData();
            SceneLoader.Instance.LoadSceneAsync(SceneID.MainMenuScene);
        }

        public void OnClickExit()
        {
            PlayerDataManager.Instance.DeletePlayerData();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
