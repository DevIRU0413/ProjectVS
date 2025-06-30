using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

using DialogueManagerClass = ProjectVS.Dialogue.DialogueManager.DialogueManager;

namespace ProjectVS.Shop.ExclamationMarkIndicator
{
    public class ExclamationMarkIndicator : MonoBehaviour
    {
        [SerializeField] private DialogueManagerClass _dialogueManager;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private void Awake()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }


        private void OnEnable()
        {
            CheckCanShowMark();
        }

        public void CheckCanShowMark()
        {
            if (_dialogueManager.CanShowEventDialogue())
                _spriteRenderer.enabled = true;
            else
                _spriteRenderer.enabled = false;
        }
    }
}
