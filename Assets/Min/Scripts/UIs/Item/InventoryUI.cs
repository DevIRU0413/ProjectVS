using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemSlotUIClass = ProjectVS.UIs.Item.ItemSlotUI.ItemSlotUI;
using BuyItemObjBehaviourClass = ProjectVS.Item.BuyItemObjBehaviour.BuyItemObjBehaviour;
using GetItemButtonBehaviourClass = ProjectVS.Item.GetItemButtonBehaviour.GetItemButtonBehaviour;
using System;
using ProjectVS.Item.ItemManager;
using ProjectVS.Item;


namespace ProjectVS.UIs.Item.InventoryUI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private List<ItemSlotUIClass> _uiSlots = new();

        [SerializeField] private List<BuyItemObjBehaviourClass> _buyObjList;
        [SerializeField] private List<GetItemButtonBehaviourClass> _getButtonList;



        private void OnEnable()
        {
            foreach (var buy in _buyObjList)
                buy.OnBuyItem += RenewUISlots;

            foreach (var get in _getButtonList)
                get.OnGetItem += RenewUISlots;
        }

        private void OnDisable()
        {
            foreach (var buy in _buyObjList)
                buy.OnBuyItem -= RenewUISlots;

            foreach (var get in _getButtonList)
                get.OnGetItem -= RenewUISlots;
        }

        private void RenewUISlots()
        {
            List<ItemData> inventory = ItemManager.Instance.Inventory.GetAllItems();

            for (int i = 0; i < _uiSlots.Count; i++)
            {
                if (i < inventory.Count)
                    _uiSlots[i].SetIcon(inventory[i].ItemIcon);
                else
                    _uiSlots[i].RemoveIcon();
            }
        }
    }
}
