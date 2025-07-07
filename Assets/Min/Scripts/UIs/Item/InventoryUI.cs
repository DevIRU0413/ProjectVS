using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ItemSlotUIClass = ProjectVS.UIs.Item.ItemSlotUI.ItemSlotUI;
using BuyItemObjBehaviourClass = ProjectVS.Item.BuyItemObjBehaviour.BuyItemObjBehaviour;
using GetItemButtonBehaviourClass = ProjectVS.Item.GetItemButtonBehaviour.GetItemButtonBehaviour;
using System;
using ProjectVS.Item.ItemManager;
using ProjectVS.Item;
using UniqueItemSlotUIClass = ProjectVS.UIs.Item.UniqueItemSlotUI.UniqueItemSlotUI;
using System.Linq;


namespace ProjectVS.UIs.Item.InventoryUI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField] private List<ItemSlotUIClass> _uiSlots = new();
        [SerializeField] private UniqueItemSlotUIClass _uniqueSlot;

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

        private void Start()
        {
            RenewUISlots();
        }

        public void RenewUISlots()
        {
            List<ItemData> inventory = ItemManager.Instance.Inventory.GetAllItems();

            // 유니크 아이템 분리
            ItemData uniqueItem = inventory.Find(item => item.ItemRank == ItemRank.Unique);

            if (uniqueItem != null)
            {
                Debug.Log($"[InventoryUI] 유니크 아이템 인벤토리 추가");
                _uniqueSlot.SetIcon(uniqueItem.ItemIcon);
            }
            else
            {
                _uniqueSlot.RemoveIcon();
                Debug.Log($"[InventoryUI] 유니크 아이템이 인벤토리에 없음");
            }

            // 일반 아이템만 필터링
            List<ItemData> normalItems = inventory
                .Where(item => item.ItemRank != ItemRank.Unique)
                .ToList();

            for (int i = 0; i < _uiSlots.Count; i++)
            {
                if (i < normalItems.Count)
                    _uiSlots[i].SetIcon(normalItems[i].ItemIcon);
                else
                    _uiSlots[i].RemoveIcon();
            }
        }
    }
}
