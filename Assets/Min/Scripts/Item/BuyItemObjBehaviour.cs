using System.Collections;
using System.Collections.Generic;

using ProjectVS.Manager;

using TMPro;

using UnityEngine;


namespace ProjectVS.Item.BuyItemObjBehaviour
{
    public class BuyItemObjBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private GameObject _xImage;

        [SerializeField] private LayerMask _playerMask;

        private ItemCombinator _itemCombinator;
        private ItemInventory _itemInventory;
        private ItemData _itemData;

        private bool _isPurchased = false;

        public void Init(ItemData data, ItemCombinator combinator, ItemInventory inventory)
        {
            _itemData = data;
            _itemCombinator = combinator;
            _itemInventory = inventory;
            _isPurchased = false;

            _xImage.SetActive(false);
            RenewObjAppearance();
        }

        private void RenewObjAppearance()
        {
            _spriteRenderer.sprite = _itemData.ItemIcon;
            _priceText.text = _itemData.ItemValue.ToString();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _playerMask) != 0)
            {
                if (PlayerDataManager.Instance.Gold < _itemData.ItemValue) return; // 돈이 없으면 return
                if (_isPurchased) return; // 이미 구매했으면 return

                // 조합 아이템인지 확인
                if (_itemData.ItemRank == ItemRank.Composite)
                {
                    int id1 = _itemData.ItemAddID1;
                    int id2 = _itemData.ItemAddID2;

                    // 두 재료 아이템이 인벤토리에 있는지 확인
                    if (_itemInventory.HasItem(id1) && _itemInventory.HasItem(id2))
                    {
                        // 둘 다 Max 레벨인지 확인
                        //if (_itemInventory.최대레벨인지(id1) && _itemInventory.최대레벨인지(id2))
                        //{
                        // 조합 시도
                        if (_itemCombinator.TryCombine(id1, id2, out ItemData result))
                        {
                            // 기존 아이템 제거
                            _itemInventory.RemoveItem(ItemDatabase.Instance.GetItem(id1));
                            _itemInventory.RemoveItem(ItemDatabase.Instance.GetItem(id2));

                            // 조합 아이템 추가
                            _itemInventory.AddItem(result);
                        }
                        else
                        {
                            // Debug.LogWarning("[GetItemButtonBehaviour] 둘 다 재료이면서 최대레벨이지만 조합 실패");
                            // _itemInventory.레벨업(_itemData);
                            // 아니면 _itemData.현재레벨++;
                        }
                        //}
                    }
                    else
                    {
                        // 단순 레벨업
                        // _itemInventory.레벨업(_itemData);
                        // 아니면 _itemData.현재레벨++;
                    }

                    _isPurchased = true;
                    ChangeToDeactivation();
                }
            }
        }

        private void ChangeToDeactivation()
        {
            _spriteRenderer.color = new Color(0f, 0f, 0f, 0.5f);
            _xImage.SetActive(true);
        }
    }
}
