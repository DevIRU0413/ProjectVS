using System.Collections;
using System.Collections.Generic;

using ProjectVS.Dialogue.DialogueManager;
using ProjectVS.Utils.UIManager;

using UnityEngine;

using DialogueManagerClass = ProjectVS.Dialogue.DialogueManager.DialogueManager;

namespace ProjectVS.Managers.TestStageManager
{
    public class TestStageManager : MonoBehaviour
    {
        [SerializeField] private DialogueManagerClass _dialogueManager;


        [ContextMenu("Test Stage Clear Event")]
        private void CheckAnyEventWhenStageClear()
        {
            if (!_dialogueManager.CanShowStageClearDialogue())
            {
                Debug.Log("[TestStageManager] 출력 가능한 스테이지 클리어 대사가 없습니다.");
                return;
            }
            else
            {
                Debug.Log("[TestStageManager] 출력 가능한 스테이지 클리어 대사가 있습니다. 대사 출력 시작.");
                UIManager.Instance.Show("Event Panel");
                _dialogueManager.ShowStageClearDialogue();
            }
        }

        [ContextMenu("Test Shop Enter Event")]
        private void CheckAnyEventWhenEnterShop()
        {
            if (!_dialogueManager.CanShowShopEnterDialogue())
            {
                Debug.Log("[TestStageManager] 출력 가능한 상점 입장 대사가 없습니다.");
                return;
            }
            else
            {
                Debug.Log("[TestStageManager] 출력 가능한 상점 입장 대사가 있습니다. 대사 출력 시작.");
                UIManager.Instance.Show("Event Panel");
                _dialogueManager.ShowShopEnterDialogue();
            }
        }


        [ContextMenu("Test Before Final Stage Event")]
        private void CheckAnyEventWhenBeforeFinalStage()
        {
            if (!_dialogueManager.CanShowBeforeFinalStageDialogue())
            {
                Debug.Log("[TestStageManager] 출력 가능한 최종 스테이지 전 대사가 없습니다.");
                return;
            }
            else
            {
                Debug.Log("[TestStageManager] 출력 가능한 최종 스테이지 전 대사가 있습니다. 대사 출력 시작.");
                UIManager.Instance.Show("Event Panel");
                _dialogueManager.ShowBeforeFinalStageDialogue();
            }
        }
    }
}
