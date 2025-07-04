using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ProjectVS.Item;
using RecipePanelButtonsClass = ProjectVS.UIs.PanelBehaviours.RecipePanelButtons.RecipePanelButtons;


namespace ProjectVS.UIs.ItemButtonBehaviour
{
    public class ItemButtonBehaviour : MonoBehaviour
    {


        [SerializeField] private Image _itemIcon;

        private ItemData _data;
        private RecipePanelButtonsClass _recipePanel;
        private bool _isClickable;

        public void Init(int itemID, RecipePanelButtonsClass recipePanel, bool isClickable)
        {
            _data = ItemDatabase.Instance.GetItem(itemID);
            _recipePanel = recipePanel;
            _isClickable = isClickable;

            RenewAppearance();
        }

        private void RenewAppearance()
        {
            _itemIcon.sprite = _data.ItemIcon;
        }

        public void OnClickIconButton()
        {
            // TODO: _recipePanel의 필드에 있는 GameObject rightPanel 활성화 및 초기화
        }
    }
}
