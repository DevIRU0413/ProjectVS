using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemSlotUIClass = ProjectVS.UIs.Item.ItemSlotUI.ItemSlotUI;
using BuyItemObjBehaviourClass = ProjectVS.Item.BuyItemObjBehaviour.BuyItemObjBehaviour;
using GetItemButtonBehaviourClass = ProjectVS.Item.GetItemButtonBehaviour.GetItemButtonBehaviour;


namespace ProjectVS.UIs.Item.InventoryUI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private List<ItemSlotUIClass> _uiSlots = new();

        [SerializeField] private List<BuyItemObjBehaviourClass> _buyObjList;
        [SerializeField] private List<GetItemButtonBehaviourClass> _getButtonList;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void RenewUISlots()
        {

        }
    }
}
