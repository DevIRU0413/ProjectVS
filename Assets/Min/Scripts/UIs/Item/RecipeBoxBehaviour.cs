using System.Collections;
using System.Collections.Generic;

using ProjectVS.Item;

using UnityEngine;
using RecipeBoxHolderClass = ProjectVS.UIs.Item.RecipeBoxHolder.RecipeBoxHolder;
using RecipePanelButtonsClass = ProjectVS.UIs.PanelBehaviours.RecipePanelButtons.RecipePanelButtons;
using ItemButtonBehaviourClass = ProjectVS.UIs.Item.ItemButtonBehaviour.ItemButtonBehaviour;
using TMPro;

namespace ProjectVS.UIs.Item.RecipeBoxBehaviour
{
    public class RecipeBoxBehaviour : MonoBehaviour
    {
        [SerializeField] private ItemButtonBehaviourClass _compositeItemButton;
        [SerializeField] private ItemButtonBehaviourClass _baseItemButton1;
        [SerializeField] private ItemButtonBehaviourClass _baseItemButton2;


        private RecipeBoxHolderClass _boxHolder;

        [SerializeField] private TMP_Text _itemNameText;


        private ItemData _item;

        public void Init(ItemData item, RecipeBoxHolderClass boxHolder)
        {
            _item = item;
            _boxHolder = boxHolder;

            RenewBoxAppearance();
            CreateItemIconButtons();
        }

        private void RenewBoxAppearance()
        {
            _itemNameText.text = _item.ItemName;
        }

        private void CreateItemIconButtons()
        {
            _compositeItemButton.Init(_item.ItemID, _boxHolder, true);
            _baseItemButton1.Init(_item.ItemAddID1, _boxHolder, true);
            _baseItemButton2.Init(_item.ItemAddID2, _boxHolder, true);
        }
    }
}
