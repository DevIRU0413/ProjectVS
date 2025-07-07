using System.Collections;
using System.Collections.Generic;

using ProjectVS.Dialogue.TextEffect.DialogueTextTyper;
using DialogueManagerClass = ProjectVS.Dialogue.DialogueManager.DialogueManager;
using UnityEngine;


namespace ProjectVS.Dialogue.DialogueSceneTextAssigner
{
    public class DialogueSceneTextAssigner : MonoBehaviour
    {
        [SerializeField] private DialogueTextTyper _shopEnterText;
        [SerializeField] private DialogueTextTyper _repeatText;
        [SerializeField] private DialogueTextTyper _eventText;
        [SerializeField] private DialogueTextTyper _stageClearText;

        public void Start()
        {
            AssignText();
        }

        private void AssignText()
        {
            DialogueManagerClass.Instance.AssignTextWhenSceneChanged(_shopEnterText, _repeatText, _eventText, _stageClearText);
        }
    }
}
