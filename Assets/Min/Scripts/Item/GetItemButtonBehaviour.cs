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

        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _descriptionText;

        private ItemCombinator _itemCombinator;
        private ItemInventory _itemInventory;
        private ItemData _itemData;

        private bool _isSelected = false;


        public void Init(ItemData data, ItemCombinator combinator, ItemInventory inventory)
        {
            _itemData = data;
            _itemCombinator = combinator;
            _itemInventory = inventory;
            _isSelected = false;

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

            foreach (int pairId in pairCandidates)
            {
                // 2. 인벤토리에 해당 ID가 있고, 조건 만족하는지 검사
                List<ItemData> candidates = _itemInventory.GetItemsByID(pairId);

                foreach (var other in candidates)
                {
                    bool isUsable = other.ItemCurLevel >= other.ItemMaxLevel && !other.IsComposited;

                    if (isUsable)
                    {
                        // 3. TryCombine 시도
                        if (_itemCombinator.TryCombine(thisId, pairId, out ItemData result))
                        {
                            Debug.Log($"[조합 시도] {thisId} + {pairId} = {result.ItemName}");

                            // 4. 현재 아이템과 조합 상대 아이템 제거
                            _itemInventory.RemoveItem(_itemData);
                            other.IsComposited = true;
                            _itemInventory.RemoveItem(other);

                            // 5. 조합 아이템 추가
                            _itemInventory.AddItem(result);

                            Debug.Log($"[OnClickGetButton] 조합 성공, {result.ItemName} 획득");

                            _isSelected = true;
                            UIManager.Instance.ForceCloseTopPanel();
                            return;
                        }
                    }
                }
            }

            // 조건을 만족하는 조합 상대가 없으면 레벨업
            Debug.Log("[OnClickGetButton] 조합 조건 없음 → 그냥 레벨업");
            _itemData.ItemLevelUp();
            _isSelected = true;
            UIManager.Instance.ForceCloseTopPanel();
        }
    }
}
