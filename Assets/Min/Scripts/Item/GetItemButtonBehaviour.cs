using System.Collections;
using System.Collections.Generic;

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
            if ((int)_itemData.ItemEffect == 7) _descriptionText.text = $"방어력 + {_itemData.ItemEffectValue}";
            if ((int)_itemData.ItemEffect == 8) _descriptionText.text = $"이동속도 + {_itemData.ItemEffectValue}";
        }

        public void OnClickGetButton()
        {
            if (_isSelected) return;

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

                _isSelected = true;
            }

            UIManager.Instance.ForceCloseTopPanel();
        }
    }
}
