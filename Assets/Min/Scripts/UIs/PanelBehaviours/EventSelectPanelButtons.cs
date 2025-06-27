using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;
using DialogueManagerClass = ProjectVS.Dialogue.DialogueManager.DialogueManager;
using UnityEngine.UI;


namespace ProjectVS.UIs.PanelBehaviours.EventSelectPanelButtons
{
    public class EventSelectPanelButtons : MonoBehaviour
    {
        [SerializeField] private DialogueManagerClass _dialogueManager;
        [SerializeField] private Button _eventButton;

        private ColorBlock _eventButtonColors;
        private Color _clickableNormalColor;
        private Color _unClickableNormalColor;

        private void Awake()
        {
            InitColor();
        }

        private void OnEnable()
        {
            _dialogueManager.ShowRepeatDialogue();

            CheckDisableButton();

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

        public void OnClickChangeCostumeButton()
        {
            UIManager.Instance.Show("Costume Change Panel");
        }

        private void InitColor()
        {
            _clickableNormalColor = _eventButton.colors.selectedColor;
            _unClickableNormalColor = new Color(0.3f, 0.3f, 0.3f, 1f); // 회색 임의 지정

            _eventButtonColors = _eventButton.colors;
            _eventButtonColors.disabledColor = _unClickableNormalColor;
            _eventButton.colors = _eventButtonColors;
        }

        private void CheckDisableButton()
        {
            if (!_dialogueManager.CanShowEventDialogue())
            {
                _eventButton.interactable = false;
            }
            else
            {
                _eventButton.interactable = true;
            }
        }
    }
}
