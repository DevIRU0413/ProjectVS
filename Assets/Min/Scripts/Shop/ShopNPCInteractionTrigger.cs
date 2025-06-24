using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectVS.Shop.ShopNPCInteractionTrigger
{
    public class ShopNPCInteractionTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private GameObject _eventSelectPanel;


        private bool _canInteract = false;
        public bool CanInteract => _canInteract;    // 해당 프로퍼티가 true일 때만 상호작용 가능하도록 프로퍼티 사용


        private void Awake()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
            {
                _canInteract = true;

                _spriteRenderer.enabled = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _playerLayer) != 0)
            {
                _canInteract = false;

                _spriteRenderer.enabled = false;
            }
        }
    }
}
