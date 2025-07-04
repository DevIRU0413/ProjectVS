using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;
using ProjectVS.Dialogue.DialogueManager;
using ProjectVS.Dialogue.DialogueManagerR;

namespace ProjectVS.Managers.TestStageManager
{
    public class TestStageManager : MonoBehaviour
    {
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
