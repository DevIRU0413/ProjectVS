using System.Collections;
using System.Collections.Generic;

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
        }

        private void HandleCompositeItem()
        {
            int thisId = _itemData.ItemID;
            List<int> pairCandidates = _itemCombinator.GetAllPossiblePairs(thisId);

            // 실제 인벤토리에 존재하고 조건을 만족하는 조합 후보 수집
            List<(ItemData other, ItemData result)> validCombinations = new();

            foreach (int pairId in pairCandidates)
            {
                List<ItemData> candidates = _itemInventory.GetItemsByID(pairId);

                foreach (var other in candidates)
                {
                    bool isUsable = other.ItemCurLevel >= other.ItemMaxLevel && !other.IsComposited;

                    if (isUsable && _itemCombinator.TryCombine(thisId, pairId, out ItemData result))
                    {
                        validCombinations.Add((other, result));
                    }
                }
            }

            // 후보가 하나라도 있으면 랜덤으로 선택해 조합
            if (validCombinations.Count > 0)
            {
                var selected = validCombinations[Random.Range(0, validCombinations.Count)];
                ItemData other = selected.other;
                ItemData result = selected.result;

                Debug.Log($"[BuyItemObj] 조합 성공: {thisId} + {other.ItemID} = {result.ItemName}");

                _itemInventory.RemoveItem(_itemData); // 구매 아이템 제거
                other.IsComposited = true;
                _itemInventory.RemoveItem(other);     // 조합 상대 제거
                _itemInventory.AddItem(result);       // 조합 결과 추가
            }
            else
            {
                Debug.Log("[BuyItemObj] 조합 조건 없음 → 그냥 레벨업");
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
