using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;
using UnityEngine.UI;



namespace ProjectVS.Dialogue.ChoiceDialogueManager
{
    public class ChoiceDialogueManager : MonoBehaviour
    {
        [Header("선택창 오브젝트 등록")]
        [SerializeField] GameObject _choiceButtonsPanel;
        [SerializeField] Button _upperChoiceButton;
        [SerializeField] Button _lowerChoiceButton;
        [SerializeField] TMP_Text _upperChoiceText;
        [SerializeField] TMP_Text _lowerChoiceText;

        private Action _onUpperClick;
        private Action _onLowerClick;

        public void ShowChoiceButtons(string upperContent, string lowerContent, Action onUpperClick, Action onLowerClick)
        {
            Debug.Log($"[ChoiceDialogueManager] ShowChoiceButtons 메서드 호출 됨");

            _choiceButtonsPanel.SetActive(true);

            _upperChoiceText.text = upperContent;
            _lowerChoiceText.text = lowerContent;

            _onUpperClick = onUpperClick;
            _onLowerClick = onLowerClick;

            _upperChoiceButton.onClick.RemoveAllListeners();
            _lowerChoiceButton.onClick.RemoveAllListeners();

            _upperChoiceButton.onClick.AddListener(() =>
            {
                _onUpperClick?.Invoke();
                _choiceButtonsPanel.SetActive(false);
            });
            _lowerChoiceButton.onClick.AddListener(() =>
            {
                _onLowerClick?.Invoke();
                _choiceButtonsPanel.SetActive(false);
            });
        }
    }
}
