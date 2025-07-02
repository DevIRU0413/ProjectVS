using System;

using ProjectVS.Scene;
using ProjectVS.Util;

using ProjectVS.Interface;

using UnityEngine;

namespace ProjectVS.Manager
{
    /// <summary>
    /// 게임의 전역 상태와 씬 관리만을 담당하는 싱글톤
    /// </summary>
    public class GameManager : SimpleSingleton<GameManager>, IManager
    {
        [field: SerializeField] public GamePlayType GamePlayType { get; private set; } = GamePlayType.Build;
        [field: SerializeField] public GameState CurrentState { get; private set; } = GameState.Play;
        [field: SerializeField] public SceneID CurrentSceneID { get; private set; } = SceneID.None;

        public int Priority => (int)ManagerPriority.GameManager;
        public bool IsDontDestroy => IsDontDestroyOnLoad;

        public event Action<GameState> OnStateChanged;
        public event Action<SceneID> OnSceneChanged;
        public event Action<GamePlayType> OnPlayTypeChanged;

        public void Initialize()
        {
            InitCurrentSceneID();
        }
        public void Cleanup()
        {
        }
        public GameObject GetGameObject() => this.gameObject;

        /// <summary>
        /// 씬 ID를 초기화합니다 (SceneBase에서 추출)
        /// </summary>
        private void InitCurrentSceneID()
        {
            if (CurrentSceneID != SceneID.None) return;

            GameObject sceneBase = GameObject.FindGameObjectWithTag("SceneBase");
            if (sceneBase != null && sceneBase.TryGetComponent(out SceneBase cmp))
                CurrentSceneID = cmp.SceneID;
        }

        /// <summary>
        /// 게임 상태를 설정합니다.
        /// </summary>
        public void SetState(GameState newState)
        {
            if (CurrentState == newState) return;

            CurrentState = newState;
            Debug.Log($"[GameManager] 상태 전환: {newState}");
            OnStateChanged?.Invoke(newState);
        }

        /// <summary>
        /// 플레이 타입을 설정합니다.
        /// </summary>
        public void SetPlayType(GamePlayType newType)
        {
            if (GamePlayType == newType) return;

            GamePlayType = newType;
            Debug.Log($"[GameManager] 플레이 타입 변경: {newType}");
            OnPlayTypeChanged?.Invoke(newType);
        }

        /// <summary>
        /// 씬을 전환합니다.
        /// </summary>
        public void SetScene(SceneID newSceneID)
        {
            if (CurrentSceneID == newSceneID || newSceneID == SceneID.None) return;

            var old = CurrentSceneID;
            CurrentSceneID = newSceneID;

            Debug.Log($"[GameManager] 씬 전환: {old} > {newSceneID}");
            OnSceneChanged?.Invoke(newSceneID);

            SceneLoader.Instance.LoadSceneAsync(newSceneID);
        }
    }
}
