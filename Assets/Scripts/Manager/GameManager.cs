using System;

using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Manager
{
    // 담당 기능
    // 1. 상태 관리
    // 2. 씬 전환
    public class GameManager : SimpleSingleton<GameManager>
    {
        [field: SerializeField] public GamePlayType PlayType { get; private set; } = GamePlayType.Build;
        [field: SerializeField] public GameState CurrentState { get; private set; } = GameState.Play;
        [field: SerializeField] public SceneID CurrentSceneID { get; private set; } = SceneID.MainMenuScene;

        public event Action<GameState> OnStateChanged;
        public event Action<SceneID> OnSceneChanged;

        protected override void Awake()
        {
            CurrentState = GameState.Play;

            // 강제 생성 진행 해야될듯

            // 플레이어 데이터 매니저
            var playerData = PlayerDataManager.ForceInstance;

            // UI 매니저

            // Audio 매니저
            var audio = AudioManager.ForceInstance;

            // Scene Loader
            var sceneLoader = SceneLoader.ForceInstance;
        }

        // 상테 전환
        public void SetState(GameState newState)
        {
            if (CurrentState == newState) return;
            CurrentState = newState;

            Debug.Log($"[GameManager] 상태 전환: {newState}");
            OnStateChanged?.Invoke(newState);
        }

        // 씬 전환
        public void SetSceneID(SceneID sceneID)
        {
            if (CurrentSceneID == sceneID) return;
            if (SceneID.None == sceneID) return;

            CurrentSceneID = sceneID;

            Debug.Log($"[GameManager] 씬 전환: {CurrentSceneID}");
            OnSceneChanged?.Invoke(CurrentSceneID);

            SceneLoader.Instance.LoadSceneAsync(CurrentSceneID);
        }
    }
}
