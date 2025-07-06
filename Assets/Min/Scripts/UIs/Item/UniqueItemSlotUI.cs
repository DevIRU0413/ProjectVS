using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;


namespace ProjectVS.UIs.Item.UniqueItemSlotUI
{
    public class UniqueItemSlotUI : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Sprite _nullSprite;


        public void SetIcon(Sprite icon)
        {
            _icon.sprite = icon;
        }

        public void RemoveIcon()
        {
            _icon.sprite = _nullSprite;
        }
    }
}
