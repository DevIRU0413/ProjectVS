using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using ProjectVS.Data;
using ProjectVS.Utils.UIManager;

using TMPro;

using UnityEngine;
using UnityEngine.UI;


namespace ProjectVS.Item.GetItemButtonBehaviour
{
    public class GetItemButtonBehaviour : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Sprite _soldOutSprite;

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;

        private string _soldOutText = "품절";

        private ItemCombinator _itemCombinator;
        private ItemInventory _itemInventory;
        private ItemData _itemData;

        private bool _isSelected = false;
        private bool _isSoldOut = false;

        public event Action OnGetItem;

        public void Init(ItemData data, ItemCombinator combinator, ItemInventory inventory)
        {
            if (data == null)
            {
                _iconImage.sprite = _soldOutSprite;
                _nameText.text = _soldOutText;
                _descriptionText.text = _soldOutText;
                _isSelected = false;
                _isSoldOut = true;

                return;
            }

            _itemData = data;
            _itemCombinator = combinator;
            _itemInventory = inventory;
            _isSelected = false;
            _isSoldOut = false;

            RenewButtonAppearance();
        }

        private void RenewButtonAppearance()
        {
            _iconImage.sprite = _itemData.ItemIcon;

            _nameText.text = _itemData.ItemName;

            if ((int)_itemData.ItemEffect == 6) _descriptionText.text = $"체력 + {_itemData.ItemEffectValue}";
            else if ((int)_itemData.ItemEffect == 7) _descriptionText.text = $"방어력 + {_itemData.ItemEffectValue}";
            else if ((int)_itemData.ItemEffect == 8) _descriptionText.text = $"이동속도 + {_itemData.ItemEffectValue}";
            else _descriptionText.text = "";
        }

        public void OnClickGetButton()
        {
            if (_isSelected) return;
            if (_isSoldOut) return;

            switch (_itemData.ItemRank)
            {
                case ItemRank.Sub:
                    if (_itemData.ItemCurLevel == 0)
                        _itemInventory.AddItem(_itemData);

                    HandleCompositeItem();
                    break;
                case ItemRank.Unique:
                    Debug.Log("[OnClickGetButton] 유니크 아이템이라 그냥 레벨업");
                    if (_itemData.ItemCurLevel == 0)
                        _itemInventory.AddItem(_itemData);

                    _itemData.ItemLevelUp();
                    break;
                case ItemRank.Composite:
                    Debug.Log("[OnClickGetButton] 이미 조합된 아이템이라 그냥 레벨업");
                    if (_itemData.ItemCurLevel == 0)
                        _itemInventory.AddItem(_itemData);

                    _itemData.ItemLevelUp();
                    break;
                default:
                    Debug.LogWarning($"[OnClickGetButton] 들어오면 안되는 아이템이 클릭됨: {_itemData.ItemName}, {_itemData.ItemRank}, {_itemData.ItemID}");
                    break;
            }

            _isSelected = true;
            UIManager.Instance.ForceCloseTopPanel();

            OnGetItem?.Invoke();
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

                _itemData.IsComposited = true;               // 조합 처리
                selected.other.IsComposited = true;

                _itemInventory.RemoveItem(_itemData);        // 현재 아이템 제거
                _itemInventory.RemoveItem(selected.other);   // 조합된 다른 아이템 제거

                _itemInventory.AddItem(selected.result);     // 결과 아이템 추가
                selected.result.ItemLevelUp();               // 1레벨 부터 시작
                Debug.Log($"[조합 성공] {_itemData.ItemName} + {selected.other.ItemName} → {selected.result.ItemName}");
            }
            else
            {
                // 5. 조합 조건을 만족하지 않으면 그냥 레벨업
                Debug.Log("[조합 실패] 조건 만족 아이템 없음 → 레벨업");
                _itemData.ItemLevelUp();
            }

            _isSelected = true;
            UIManager.Instance.ForceCloseTopPanel();
        }
    }
}
