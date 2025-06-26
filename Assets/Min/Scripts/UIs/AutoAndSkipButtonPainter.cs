using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ProjectVS.Dialogue.TextEffect.DialogueTextTyper;
using ProjectVS.Utils.UIManager;
using DialogueManagerClass = ProjectVS.Dialogue.DialogueManager.DialogueManager;

public class AutoAndSkipButtonPainter : MonoBehaviour
{
    [SerializeField] private Image _skipButtonImage;
    [SerializeField] private Image _autoButtonImage;

    [SerializeField] private DialogueTextTyper _eventDialogueTextTyper;
    [SerializeField] private DialogueManagerClass _dialogueManager;

    private Color _toggledColor = new Color(0.4f, 0.9f, 0.95f, 1f); // 하늘색 임의 지정

    private void OnEnable()
    {        
        _eventDialogueTextTyper.IsSkipToggled.Subscribe(CheckSkipToggled);
        _dialogueManager.IsAutoMode.Subscribe(CheckAutoToggled);
    }

    private void Start()
    {
        CheckSkipToggled();
        CheckAutoToggled();
    }

    private void OnDisable()
    {
        if (_eventDialogueTextTyper != null)
        {
            _eventDialogueTextTyper.IsSkipToggled.Unsubscribe(CheckSkipToggled);
        }
        if (_dialogueManager != null)
        {
            _dialogueManager.IsAutoMode.Unsubscribe(CheckAutoToggled);
        }
    }


    private void CheckSkipToggled()
    {
        if (_eventDialogueTextTyper.IsSkipToggled.Value)
        {
            _skipButtonImage.color = _toggledColor;
        }
        else
        {
            _skipButtonImage.color = Color.white;
        }
    }

    private void CheckAutoToggled()
    {
        if (_dialogueManager.IsAutoMode.Value)
        {
            _autoButtonImage.color = _toggledColor;
        }
        else
        {
            _autoButtonImage.color = Color.white;
        }
    }
}
