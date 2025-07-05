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
            // 중복 선택 방지
            if (_isSelected) return;

            // 현재 버튼이 조합 아이템이라면
            if (_itemData.ItemRank == ItemRank.Sub)
            {
                int id1 = _itemData.ItemAddID1;
                int id2 = _itemData.ItemAddID2;

                // 두 재료 아이템이 인벤토리에 존재하는지 확인
                bool hasItem1 = _itemInventory.HasItem(id1);
                bool hasItem2 = _itemInventory.HasItem(id2);

                // 두 재료 아이템 모두 최대 레벨이며, 조합된 적이 없는지 확인
                bool isMax1 = _itemInventory.GetItemsByID(id1).All(i => i.ItemCurLevel >= i.ItemMaxLevel && !i.IsComposited);
                bool isMax2 = _itemInventory.GetItemsByID(id2).All(i => i.ItemCurLevel >= i.ItemMaxLevel && !i.IsComposited);

                // -- 디버그 로그
                var items1 = _itemInventory.GetItemsByID(id1);
                var items2 = _itemInventory.GetItemsByID(id2);

                // 각 아이템의 이름을 쉼표로 이어붙임
                string itemNames1 = string.Join(", ", items1.Select(i => i.ItemName));
                string itemNames2 = string.Join(", ", items2.Select(i => i.ItemName));

                // 조건 로그 출력
                Debug.Log($"[조건 체크] item1(id:{id1}): [{itemNames1}] hasItem1: {hasItem1}, isMax1: {isMax1}");
                Debug.Log($"[조건 체크] item2(id:{id2}): [{itemNames2}] hasItem2: {hasItem2}, isMax2: {isMax2}");
                // -- 디버그 로그

                // 조합 조건이 모두 충족되었을 경우
                if (hasItem1 && hasItem2 && isMax1 && isMax2)
                {
                    if (_itemCombinator.TryCombine(id1, id2, out ItemData result))
                    {
                        var itemList1 = _itemInventory.GetItemsByID(id1);
                        var itemList2 = _itemInventory.GetItemsByID(id2);

                        foreach (var i in itemList1)
                        {
                            if (!i.IsComposited && i.ItemCurLevel >= i.ItemMaxLevel)
                            {
                                i.IsComposited = true;
                                _itemInventory.RemoveItem(i);
                                Debug.Log($"[OnClickGetButton] {i.ItemName}이 조합되어 삭제됨");
                                break;
                            }
                        }

                        foreach (var i in itemList2)
                        {
                            if (!i.IsComposited && i.ItemCurLevel >= i.ItemMaxLevel)
                            {
                                i.IsComposited = true;
                                _itemInventory.RemoveItem(i);
                                Debug.Log($"[OnClickGetButton] {i.ItemName} 삭제됨");
                                break;
                            }
                        }

                        Debug.Log($"[OnClickGetButton] {result.ItemName} 추가됨");
                        _itemInventory.AddItem(result);

                        Debug.Log($"[인벤토리 상태]");
                        foreach (var item in _itemInventory.GetAllItems())
                            Debug.Log($"{item.ItemName} (LV: {item.ItemCurLevel}, ID: {item.ItemID})");
                    }
                    else
                    {
                        Debug.LogWarning("[OnClickGetButton] TryCombine 실패 - 조합 조건은 충족했으나 실패");
                    }
                }
                else
                {
                    Debug.Log("[OnClickGetButton] 조합실패해서 그냥 레벨업");
                    _itemData.ItemLevelUp();
                }

                _isSelected = true;
                UIManager.Instance.ForceCloseTopPanel();
            }
            else if (_itemData.ItemRank == ItemRank.Unique)
            {
                Debug.Log("[OnClickGetButton] 유니크 아이템이라 그냥 레벨업");
                _itemData.ItemLevelUp();
                UIManager.Instance.ForceCloseTopPanel();
            }
            else
            {
                Debug.Log("[OnClickGetButton] 이미 조합된 아이템이라 그냥 레벨업");
                _itemData.ItemLevelUp();
                UIManager.Instance.ForceCloseTopPanel();
            }
        }
    }
}
