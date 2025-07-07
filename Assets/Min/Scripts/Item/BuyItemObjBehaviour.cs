using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ProjectVS.Manager;

using TMPro;

using UnityEngine;
using UnityEngine.UI;


namespace ProjectVS.Item.BuyItemObjBehaviour
{
    public class BuyItemObjBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private TMP_Text _priceText;
        [SerializeField] private Sprite _soldOutSprite;


        [SerializeField] private LayerMask _playerMask;

        private string SOLDOUT_TEXT = "품절";

        private ItemCombinator _itemCombinator;
        private ItemInventory _itemInventory;
        private ItemData _itemData;

        private bool _isPurchased = false;
        private bool _isSoldOut = false;

        public event Action OnBuyItem;

        public void Init(ItemData data, ItemCombinator combinator, ItemInventory inventory)
        {
            // 품절 처리
            if (data == null)
            {
                _isPurchased = false;
                SetSoldOutState();
                _isSoldOut = true;

                return;
            }

            _itemData = data;
            _itemCombinator = combinator;
            _itemInventory = inventory;
            _isPurchased = false;
            _isSoldOut = false;

            RenewObjAppearance();
        }

        private void RenewObjAppearance()
        {
            _spriteRenderer.sprite = _itemData.ItemIcon;
            _priceText.text = $"$ {_itemData.ItemValue.ToString()}";
        }

        private void SetSoldOutState()
        {
            _spriteRenderer.sprite = _soldOutSprite;
            _priceText.text = SOLDOUT_TEXT;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (((1 << collision.gameObject.layer) & _playerMask) == 0) return;
            //if (PlayerDataManager.Instance.Gold < _itemData.ItemValue) return;
            if (_isPurchased) return;
            if (_isSoldOut) return;

            switch (_itemData.ItemRank)
            {
                case ItemRank.Sub:
                    HandleCompositeItem();
                    break;
                case ItemRank.Composite:
                    Debug.Log("[BuyItemObj] Composite 아이템은 상점에서 별도 조합 없이 구매 불가 → 그냥 레벨업");
                    _itemData.ItemLevelUp();
                    break;
                default:
                    Debug.LogWarning($"[OnClickGetButton] 들어오면 안되는 아이템이 클릭됨: {_itemData.ItemName}, {_itemData.ItemRank}, {_itemData.ItemID}");
                    break;
            }

            _isPurchased = true;
            ChangeToDeactivation();
            OnBuyItem?.Invoke();
        }

        private void HandleCompositeItem()
        {
            int thisId = _itemData.ItemID;

            // 1. 조합 가능한 모든 쌍 ID 얻기
            List<int> combinableIDs = _itemCombinator.GetAllPossiblePairs(thisId);

            // 2. 인벤토리에서 조건에 맞는 후보 필터링
            List<ItemData> usablePairs = _itemInventory
                .GetAllItems()
                .Where(item =>
                    combinableIDs.Contains(item.ItemID) &&
                    item.ItemCurLevel >= item.ItemMaxLevel &&
                    !item.IsComposited)
                .ToList();

            // 3. 조합 가능한 실제 쌍 찾기
            List<(ItemData other, ItemData result)> validCombinations = new();

            foreach (var other in usablePairs)
            {
                if (_itemCombinator.TryCombine(thisId, other.ItemID, out ItemData result))
                {
                    validCombinations.Add((other, result));
                }
            }

            // 4. 조합 성공
            if (validCombinations.Count > 0)
            {
                var selected = validCombinations[UnityEngine.Random.Range(0, validCombinations.Count)];

                _itemData.IsComposited = true;
                selected.other.IsComposited = true;

                _itemInventory.RemoveItem(_itemData);        // 현재 아이템 제거
                _itemInventory.RemoveItem(selected.other);   // 조합된 다른 아이템 제거

                _itemInventory.AddItem(selected.result);     // 결과 아이템 추가
                selected.result.ItemLevelUp();               // 1레벨 부터 시작

                Debug.Log($"[BuyItemObj] 조합 성공: {_itemData.ItemName} + {selected.other.ItemName} → {selected.result.ItemName}");
            }
            else
            {
                Debug.Log("[BuyItemObj] 조합 조건 없음 → 그냥 레벨업");
                _itemInventory.AddItem(_itemData);  // 인벤토리에 추가 (처음 얻는 거라면)
                _itemData.ItemLevelUp();
            }
        }

        private void ChangeToDeactivation()
        {
            if (_isSoldOut) return;

            _spriteRenderer.color = new Color(0f, 0f, 0f, 0.5f);
        }
    }
}
