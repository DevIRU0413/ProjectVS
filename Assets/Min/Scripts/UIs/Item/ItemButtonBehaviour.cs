using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ProjectVS.Item;
using RecipeBoxHolderClass = ProjectVS.UIs.Item.RecipeBoxHolder.RecipeBoxHolder;
using ProjectVS.Item.SetItemData;

namespace ProjectVS.UIs.Item.ItemButtonBehaviour
{
    public class ItemButtonBehaviour : MonoBehaviour
    {
        [SerializeField] private Image _itemIcon;
        [SerializeField] private Button _button;

        private RecipeBoxHolderClass _boxHolder;
        private ItemData _data;
        private bool _isClickable;

        private bool _isSetItem;
        private SetItemData _setData;

        private void Awake()
        {
            if (_itemIcon == null)
                _itemIcon = GetComponent<Image>();
            if (_button == null)
                _button = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClickIconButton);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClickIconButton);
        }

        public void Init(int itemID, RecipeBoxHolderClass boxHolder, bool isClickable)
        {
            _data = ItemDatabase.Instance.GetItem(itemID);
            _boxHolder = boxHolder;
            _isClickable = isClickable;
            _isSetItem = false;
            _setData = null;

            RenewAppearance();
        }

        public void InitWithSetData(SetItemData setData, RecipeBoxHolderClass boxHolder, bool isClickable)
        {
            _data = null;
            _boxHolder = boxHolder;
            _isClickable = isClickable;
            _isSetItem = true;
            _setData = setData;

            RenewAppearance();
        }

        private void RenewAppearance()
        {
            if (_isSetItem && _setData != null)
            {
                _itemIcon.sprite = _setData.SetIcon;
            }
            else if (_data != null)
            {
                _itemIcon.sprite = _data.ItemIcon;
            }
        }

        public void OnClickIconButton()
        {
            if (!_isClickable || _boxHolder == null) return;

            if (_isSetItem) _boxHolder.OpenSetPanel(_setData);
            else            _boxHolder.OpenRightItemPanel(_data);
        }
    }
}
