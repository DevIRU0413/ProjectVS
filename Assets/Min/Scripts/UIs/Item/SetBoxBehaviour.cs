using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using ItemButtonBehaviourClass = ProjectVS.UIs.Item.ItemButtonBehaviour.ItemButtonBehaviour;
using ProjectVS.Item;
using ProjectVS.Item.SetItemData;
using RecipeBoxHolderClass = ProjectVS.UIs.Item.RecipeBoxHolder.RecipeBoxHolder;


namespace ProjectVS.UIs.Item.SetOrRecipeBoxBehaviour
{
    public class SetBoxBehaviour : MonoBehaviour
    {
        [SerializeField] private ItemButtonBehaviourClass _setItemButton;
        [SerializeField] private ItemButtonBehaviourClass[] _baseItemButtons;
        [SerializeField] private TMP_Text _itemNameText;

        private RecipeBoxHolderClass _boxHolder;

        public void Init(SetItemData setItem, RecipeBoxHolderClass boxHolder)
        {
            _boxHolder = boxHolder;

            RenewBoxAppearance(setItem);
            CreateItemIconButtons(setItem);
        }

        private void RenewBoxAppearance(SetItemData setItem)
        {
            _itemNameText.text = setItem.SetName;
        }

        // 아이템 버튼 세팅
        private void CreateItemIconButtons(SetItemData setItem)
        {
            for (int i = 0; i < _baseItemButtons.Length; i++)
            {
                if (i < setItem.SetItemID.Length)
                {
                    int itemId = setItem.SetItemID[i];
                    ItemData item = ItemDatabase.Instance.GetItem(itemId);

                    if (item == null)
                    {
                        Debug.LogWarning($"[SetBoxBehaviour] 존재하지 않는 아이템 ID: {itemId}");
                        continue;
                    }

                    // 유효한 아이템이면 버튼을 초기화
                    _baseItemButtons[i].Init(item.ItemID, _boxHolder, true);
                    _baseItemButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    // 세트 구성 아이템 개수보다 버튼 수가 많을 경우, 남는 버튼은 비활성화
                    _baseItemButtons[i].gameObject.SetActive(false);
                }
            }

            // 세트 대표 버튼은 세트 자체 아이콘을 사용
            if (setItem.SetIcon != null)
            {
                _setItemButton.InitWithSetData(setItem, _boxHolder, true);
            }
            else
            {
                Debug.LogWarning($"[SetBoxBehaviour] 세트 아이콘을 찾을 수 없음");
            }
        }
    }
}
