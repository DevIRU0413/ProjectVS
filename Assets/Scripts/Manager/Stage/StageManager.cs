using ProjectVS.Data;
using ProjectVS.Interface;
using ProjectVS.Manager;
using ProjectVS.Manager.Stage;
using ProjectVS.Monster;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Stage
{
    [RequireComponent(typeof(MonsterSpawnController))]
    public class StageManager : SimpleSingleton<StageManager>, IManager, IGameStateListener
    {
        private const string TEST_STAGE_SIMPLE_DATA = "SO/Stage/Stage_Simple_Data";
        private StageDataSO _stageDataSo;

        private StageFlowMachine _flowMachine;
        private StageContext _context;

        // IManager
        public int Priority => (int)ManagerPriority.StageManager;
        public bool IsDontDestroy => IsDontDestroyOnLoad;
        public GameObject GetGameObject() => this.gameObject;
        public void Cleanup() { }
        public void Initialize()
        {
            if (GameManager.Instance.GamePlayType == GamePlayType.Test)
                _stageDataSo = Resources.Load<StageDataSO>(TEST_STAGE_SIMPLE_DATA);
            else
                _stageDataSo = Resources.Load<StageDataSO>(TEST_STAGE_SIMPLE_DATA);

            _context = StageInitializer.BuildContext(this.gameObject, _stageDataSo);
            if (_context == null)
            {
                Debug.LogError("[StageManager] StageContext 생성 실패");
                return;
            }

            _flowMachine = new StageFlowMachine(_context);
            _flowMachine.Enter();
        }

        // IGameStateListener
        public void OnGameStateChanged(GameState state)
        {
            _flowMachine?.OnGameStateChanged(state);
        }

        // Unity
        private void OnEnable()
        {
            GameManager.Instance.OnStateChanged -= OnGameStateChanged;
            GameManager.Instance.OnStateChanged += OnGameStateChanged;
        }
        private void OnDisable()
        {
            GameManager.Instance.OnStateChanged -= OnGameStateChanged;
        }
        private void Update()
        {
            _flowMachine?.Update();
        }

        
    }
}
