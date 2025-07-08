using ProjectVS.Data;
using ProjectVS.Dialogue.DialogueManagerR;
using ProjectVS.Interface;
using ProjectVS.Manager;
using ProjectVS.Manager.Stage;
using ProjectVS.Monster;
using ProjectVS.Util;
using ProjectVS.Utils.UIManager;

using UnityEngine;

namespace ProjectVS.Stage
{
    public class StageManager : SimpleSingleton<StageManager>, IManager, IGameStateListener
    {
        private const string TEST_STAGE_SIMPLE_DATA = "SO/Stage/Stage_Simple_Data";
        private StageDataSO _stageDataSo;

        private StageFlowMachine _flowMachine;
        private StageContext _context;


        public StageContext Context => _context;

        public StageFlowMachine FlowMachine => _flowMachine;

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


        [ContextMenu("Test Stage Clear Event")]
        private void CheckAnyEventWhenStageClear()
        {
            //if (!DialogueManager.Instance.CanShowStageClearDialogue())
            //{
            //    Debug.Log("[TestStageManager] 출력 가능한 스테이지 클리어 대사가 없습니다.");
            //    return;
            //}
            //else
            //{
            //    Debug.Log("[TestStageManager] 출력 가능한 스테이지 클리어 대사가 있습니다. 대사 출력 시작.");
            //    UIManager.Instance.Show("Event Panel");
            //    DialogueManager.Instance.ShowStageClearDialogue();
            //}

            if (!DialogueManagerR.Instance.CanShowDialogueByType(DialogueType.StageClear))
            {
                Debug.Log("[TestStageManager] 출력 가능한 스테이지 클리어 대사가 없습니다.");
                return;
            }
            else
            {
                Debug.Log("[TestStageManager] 출력 가능한 스테이지 클리어 대사가 있습니다. 대사 출력 시작.");
                UIManager.Instance.Show("Event Panel");
                DialogueManagerR.Instance.ShowDialogueByType(DialogueType.StageClear);
            }
        }

        [ContextMenu("Test Shop Enter Event")]
        private void CheckAnyEventWhenEnterShop()
        {
            //if (!DialogueManager.Instance.CanShowShopEnterDialogue())
            //{
            //    Debug.Log("[TestStageManager] 출력 가능한 상점 입장 대사가 없습니다.");
            //    return;
            //}
            //else
            //{
            //    Debug.Log("[TestStageManager] 출력 가능한 상점 입장 대사가 있습니다. 대사 출력 시작.");
            //    UIManager.Instance.Show("Event Panel");
            //    DialogueManager.Instance.ShowShopEnterDialogue();
            //}

            if (!DialogueManagerR.Instance.CanShowDialogueByType(DialogueType.ShopEnter))
            {
                Debug.Log("[TestStageManager] 출력 가능한 상점 입장 대사가 없습니다.");
                return;
            }
            else
            {
                Debug.Log("[TestStageManager] 출력 가능한 상점 입장 대사가 있습니다. 대사 출력 시작.");
                UIManager.Instance.Show("Event Panel");
                DialogueManagerR.Instance.ShowDialogueByType(DialogueType.ShopEnter);
            }
        }

        [ContextMenu("Test Before Final Stage Event")]
        private void CheckAnyEventWhenBeforeFinalStage()
        {
            //if (!DialogueManager.Instance.CanShowBeforeFinalStageDialogue())
            //{
            //    Debug.Log("[TestStageManager] 출력 가능한 최종 스테이지 전 대사가 없습니다.");
            //    return;
            //}
            //else
            //{
            //    Debug.Log("[TestStageManager] 출력 가능한 최종 스테이지 전 대사가 있습니다. 대사 출력 시작.");
            //    UIManager.Instance.Show("Event Panel");
            //    DialogueManager.Instance.ShowBeforeFinalStageDialogue();
            //}

            if (!DialogueManagerR.Instance.CanShowDialogueByType(DialogueType.BeforeFinalStage))
            {
                Debug.Log("[TestStageManager] 출력 가능한 최종 스테이지 전 대사가 없습니다.");
                return;
            }
            else
            {
                Debug.Log("[TestStageManager] 출력 가능한 최종 스테이지 전 대사가 있습니다. 대사 출력 시작.");
                UIManager.Instance.Show("Event Panel");
                DialogueManagerR.Instance.ShowDialogueByType(DialogueType.BeforeFinalStage);
            }
        }
    }
}
