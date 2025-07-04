using System.Collections;
using System.Collections.Generic;

using ProjectVS.Item;

using UnityEngine;

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
            _compositeItemButton.Init(_item.ItemID, _recipePanel, true);
            _baseItemButton1.Init(_item.ItemAddID1, _recipePanel, true);
            _baseItemButton2.Init(_item.ItemAddID2, _recipePanel, true);
        }
    }
}
