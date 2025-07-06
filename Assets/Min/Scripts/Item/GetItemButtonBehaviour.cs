using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
                    HandleCompositeItem();
                    break;
                case ItemRank.Unique:
                    Debug.Log("[OnClickGetButton] 유니크 아이템이라 그냥 레벨업");
                    _itemData.ItemLevelUp();
                    break;
                case ItemRank.Composite:
                    Debug.Log("[OnClickGetButton] 이미 조합된 아이템이라 그냥 레벨업");
                    _itemData.ItemLevelUp();
                    break;
                default:
                    Debug.LogWarning($"[OnClickGetButton] 들어오면 안되는 아이템이 클릭됨: {_itemData.ItemName}, {_itemData.ItemRank}, {_itemData.ItemID}");
                    break;
            }

            _isSelected = true;
            UIManager.Instance.ForceCloseTopPanel();
        }

        private void HandleCompositeItem()
        {
            int thisId = _itemData.ItemID;

            // 1. 조합 가능한 모든 쌍 조회
            List<int> pairCandidates = _itemCombinator.GetAllPossiblePairs(thisId);

            // 2. 실제 인벤토리에 존재하고 조건을 만족하는 조합 후보 수집
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

            // 3. 후보가 하나라도 있으면 랜덤으로 선택해 조합
            if (validCombinations.Count > 0)
            {
                var selected = validCombinations[Random.Range(0, validCombinations.Count)];
                ItemData other = selected.other;
                ItemData result = selected.result;

                Debug.Log($"[조합 시도] {thisId} + {other.ItemID} = {result.ItemName}");

                _itemInventory.RemoveItem(_itemData);
                other.IsComposited = true;
                _itemInventory.RemoveItem(other);
                _itemInventory.AddItem(result);

                Debug.Log($"[OnClickGetButton] 조합 성공, {result.ItemName} 획득");
            }
            else
            {
                // 4. 조건을 만족하는 조합 상대가 없으면 레벨업
                Debug.Log("[OnClickGetButton] 조합 조건 없음 → 그냥 레벨업");
                _itemData.ItemLevelUp();
            }

            _isSelected = true;
            UIManager.Instance.ForceCloseTopPanel();
        }
    }
}
