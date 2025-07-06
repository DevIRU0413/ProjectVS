using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Dialogue.DialogueManager;
using ProjectVS.Dialogue.DialogueManagerR;


namespace ProjectVS.Shop.ExclamationMarkIndicator
{
    public class ExclamationMarkIndicator : MonoBehaviour
    {
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
            //if (DialogueManager.Instance.CanShowEventDialogue())
            //    _spriteRenderer.enabled = true;
            //else
            //    _spriteRenderer.enabled = false;

            if (DialogueManagerR.Instance.CanShowDialogueByType(DialogueType.ShopEvent))
                _spriteRenderer.enabled = true;
            else
                _spriteRenderer.enabled = false;
        }
    }
}
