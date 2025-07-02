using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ThanksTextIndicatorClass = ProjectVS.Shop.ThanksTextIndicator.ThanksTextIndicator;

namespace ProjectVS.Shop.ShopItemTrigger
{
    public class ShopItemTrigger : MonoBehaviour
    {
        [SerializeField] private ThanksTextIndicatorClass _thanks;

        public ScriptableObject ItemSO;


        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private LayerMask _playerLayer;
        [SerializeField] private Sprite _xSprite; // X 표시용 스프라이트

        private bool _isPurchased = false;

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
                if (_isPurchased) return;

                // ItemSO 의 가격 판단
                // if (ItemSO.price > PlayerStats.Money) return
                // else PlayerStats.Money - ItemSO.price;

                // 상점 NPC 말풍선 팝업

                // 아이템 업그레이드
                // ItemSO?.OnClick();

                // X 표시
                _spriteRenderer.sprite = _xSprite;

                // 감사 메시지 표시
                _thanks.ShowThanksText();

                _isPurchased = true;
            }
        }
    }

}
