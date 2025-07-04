using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

using RecipePanelButtonsClass = ProjectVS.UIs.PanelBehaviours.RecipePanelButtons.RecipePanelButtons;
using ItemButtonBehaviourClass = ProjectVS.UIs.ItemButtonBehaviour.ItemButtonBehaviour;
using ProjectVS.Item;


namespace ProjectVS.UIs.SetOrRecipeBoxBehaviour
{
    public class SetBoxBehaviour : MonoBehaviour
    {
        [SerializeField] private ItemButtonBehaviourClass _setItemButton;
        [SerializeField] private ItemButtonBehaviourClass _baseItemButton1;
        [SerializeField] private ItemButtonBehaviourClass _baseItemButton2;
        [SerializeField] private ItemButtonBehaviourClass _baseItemButton3;
        [SerializeField] private ItemButtonBehaviourClass _baseItemButton4;


        [SerializeField] private TMP_Text _itemNameText;


        private ItemData _item;
        private RecipePanelButtonsClass _recipePanel;

        public void Init(ItemData item, RecipePanelButtonsClass recipePanel)
        {
            _item = item;
            _recipePanel = recipePanel;

            RenewBoxAppearance();
            CreateItemIconButtons();
        }

        private void RenewBoxAppearance()
        {
            _itemNameText.text = _item.ItemName;
        }

        private void CreateItemIconButtons()
        {
            // MEMO: 아이템 데이터 테이블에는 세트 아이템은 없는 듯??
            // 따로 매핑을 해야될 듯

            //_setItemButton.Init(_item.ItemID, _recipePanel, true);
            //_baseItemButton1.Init(_item.ItemAddID1, _recipePanel, true);
            //_baseItemButton2.Init(_item.ItemAddID2, _recipePanel, true);

            //if (_item.ItemSetNum >= 3)
            //{
            //    _baseItemButton3.Init()
            //}
        }
    }
}
