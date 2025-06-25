using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;
using DialogueManagerClass = ProjectVS.Dialogue.DialogueManager.DialogueManager;


namespace ProjectVS.UIs.PanelBehaviours.EventSelectPanelButtons
{
    public class EventSelectPanelButtons : MonoBehaviour
    {
        [SerializeField] private DialogueManagerClass _dialogueManager;

        private void OnEnable()
        {
            _dialogueManager.ShowRepeatDialogue();

            Debug.Log("[EventSelectPanelButtons] 이벤트 선택 패널 버튼 활성화됨.");
        }

        public void OnClickESCButton()
        {
            UIManager.Instance.ForceCloseTopPanel();
        }

        public void OnClickEventButton()
        {
            if (!_dialogueManager.CanShowEventDialogue())
            {
                Debug.Log("[EventSelectPanelButtons] 출력 가능한 이벤트 대사가 없습니다.");
                return;
            }

            _dialogueManager.ShowEventDialogue();

            UIManager.Instance.Hide("Event Select Panel");
            UIManager.Instance.Show("Event Panel");
        }
    }
}
