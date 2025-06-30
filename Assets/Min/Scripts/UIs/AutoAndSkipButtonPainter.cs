using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ProjectVS.Dialogue.TextEffect.DialogueTextTyper;
using ProjectVS.Dialogue.DialogueManager;

public class AutoAndSkipButtonPainter : MonoBehaviour
{
    [SerializeField] private Image _skipButtonImage;
    [SerializeField] private Image _autoButtonImage;

    [SerializeField] private DialogueTextTyper _eventDialogueTextTyper;

    private Color _toggledColor = new Color(0.4f, 0.9f, 0.95f, 1f); // 하늘색 임의 지정

    private void OnEnable()
    {        
        _eventDialogueTextTyper.IsSkipToggled.Subscribe(CheckSkipToggled);
        DialogueManager.Instance.IsAutoMode.Subscribe(CheckAutoToggled);
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
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.IsAutoMode.Unsubscribe(CheckAutoToggled);
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
        if (DialogueManager.Instance.IsAutoMode.Value)
        {
            _autoButtonImage.color = _toggledColor;
        }
        else
        {
            _autoButtonImage.color = Color.white;
        }
    }
}
