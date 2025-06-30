using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Dialogue.DialogueManager;


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
            if (DialogueManager.Instance.CanShowEventDialogue())
                _spriteRenderer.enabled = true;
            else
                _spriteRenderer.enabled = false;
        }
    }
}
