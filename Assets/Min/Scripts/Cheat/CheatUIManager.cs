using System.Collections;
using System.Collections.Generic;
using ProjectVS.Item.ItemManager;
using ProjectVS.Item;

using UnityEngine;
using ItemCheatButtonClass = ProjectVS.Cheat.ItemCheatButton.ItemCheatButton;
using InventoryUIClass = ProjectVS.UIs.Item.InventoryUI.InventoryUI;


namespace ProjectVS.Cheat.CheatUIManager
{
    public class CheatUIManager : MonoBehaviour
    {
        [SerializeField] private InventoryUIClass _inventoryUI;


        [SerializeField] private Transform _itemCheatRoot;
        [SerializeField] private GameObject _buttonPrefab;

        private Dictionary<int, ItemCheatButtonClass> _buttonDict = new();

        private void OnEnable()
        {
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        private void Start()
        {
            CreateOrReuseCheatButtons();
        }

        private void CreateOrReuseCheatButtons()
        {
            foreach (ItemData item in ItemDatabase.Instance.GetAllItems())
            {
                // 이미 만들어진 버튼이 있다면 스킵
                if (_buttonDict.ContainsKey(item.ItemID))
                    continue;

                GameObject go = Instantiate(_buttonPrefab, _itemCheatRoot);
                ItemCheatButtonClass button = go.GetComponent<ItemCheatButtonClass>();

                button.InitItemCheatButton(
                    item,
                    HandleLevelUp,
                    HandleLevelDown,
                    HandleAddItem,
                    HandleRemoveItem,
                    _inventoryUI
                );

                _buttonDict.Add(item.ItemID, button);
            }
        }

        private void HandleLevelUp(ItemData item)
        {
            Refresh(item.ItemID);
        }

        private void HandleLevelDown(ItemData item)
        {
            Refresh(item.ItemID);
        }

        private void HandleAddItem(ItemData item)
        {
            if (ItemManager.Instance.Inventory.HasItem(item.ItemID))
                return;

            ItemManager.Instance.Inventory.AddItem(item);
            Refresh(item.ItemID);
            _inventoryUI.RenewUISlots();
        }

        private void HandleRemoveItem(ItemData item)
        {
            ItemManager.Instance.Inventory.RemoveItem(item);
            _inventoryUI.RenewUISlots();
        }

        private void Refresh(int itemID)
        {
            if (_buttonDict.TryGetValue(itemID, out ItemCheatButtonClass button))
            {
                button.UpdateLevelDisplay();
            }
        }
    }
}
