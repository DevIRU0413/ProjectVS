using System.Collections;
using System.Collections.Generic;
using System.Linq;

using DG.Tweening;

using ProjectVS.Item;
using ProjectVS.Item.ItemManager;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

using InventoryUIClass = ProjectVS.UIs.Item.InventoryUI.InventoryUI;


namespace ProjectVS.Cheat.ItemCheatButton
{
    public class ItemCheatButton : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _levelText;

        [SerializeField] private Button _levelUpButton;
        [SerializeField] private Button _levelDownButton;
        [SerializeField] private Button _addButton;
        [SerializeField] private Button _removeButton;

        private InventoryUIClass _inventoryUI;

        private Color _enableColor = Color.white;
        private Color _disableColor = new Color(0.2f, 0.2f, 0.2f, 1f);

        private ItemData _itemData;
        private System.Action<ItemData> _onLevelUp;
        private System.Action<ItemData> _onLevelDown;
        private System.Action<ItemData> _onAdd;
        private System.Action<ItemData> _onRemove;

        public void InitItemCheatButton(
            ItemData itemData,
            System.Action<ItemData> onLevelUp,
            System.Action<ItemData> onLevelDown,
            System.Action<ItemData> onAdd,
            System.Action<ItemData> onRemove,
            InventoryUIClass inventoryUI)
        {
            _itemData = itemData;
            _onLevelUp = onLevelUp;
            _onLevelDown = onLevelDown;
            _onAdd = onAdd;
            _onRemove = onRemove;

            _iconImage.sprite = itemData.ItemIcon;
            _nameText.text = itemData.ItemName;
            _levelText.text = $"Lvl: {itemData.ItemCurLevel}";

            _levelUpButton.onClick.RemoveAllListeners();
            _levelDownButton.onClick.RemoveAllListeners();
            _addButton.onClick.RemoveAllListeners();
            _removeButton.onClick.RemoveAllListeners();

            _levelUpButton.onClick.AddListener(HandleLevelUpWithCombination);
            _levelDownButton.onClick.AddListener(() => _onLevelDown?.Invoke(_itemData));
            _addButton.onClick.AddListener(() => _onAdd?.Invoke(_itemData));
            _removeButton.onClick.AddListener(() => _onRemove?.Invoke(_itemData));

            _inventoryUI = inventoryUI;

            RefreshVisualState();
        }

        public void UpdateLevelDisplay()
        {
            _levelText.text = $"Lvl: {_itemData.ItemCurLevel}";
            RefreshVisualState();
            _inventoryUI.RenewUISlots();
        }

        private void RefreshVisualState()
        {
            bool hasItem = ItemManager.Instance.Inventory.HasItem(_itemData.ItemID);
            bool removedItem = ItemManager.Instance.Inventory
                .GetItemsByID(_itemData.ItemID)
                .Any(item => item.IsComposited);

            _iconImage.color = (!hasItem || removedItem) ? _disableColor : _enableColor;
        }

        private void HandleLevelUpWithCombination()
        {
            bool isInInventory = ItemManager.Instance.Inventory.HasItem(_itemData.ItemID);
            ItemInventory inventory = ItemManager.Instance.Inventory;
            ItemCombinator combinator = new(ItemDatabase.Instance.GetAllItems());

            if (!isInInventory)
            {
                Debug.LogWarning("[Cheat] 조합 실패: 인벤토리에 아이템 없음");
                return;
            }

            if (_itemData.ItemRank == ItemRank.Sub)
            {
                // 조합 가능한 대상 찾기
                List<int> pairIDs = combinator.GetAllPossiblePairs(_itemData.ItemID);

                List<ItemData> candidates = inventory.GetAllItems()
                    .Where(item =>
                        item.ItemID != _itemData.ItemID &&
                        pairIDs.Contains(item.ItemID) &&
                        item.ItemCurLevel >= item.ItemMaxLevel && // other가 풀렙
                        _itemData.ItemCurLevel >= _itemData.ItemMaxLevel && // 자신도 풀렙
                        !_itemData.IsComposited && !item.IsComposited)
                    .ToList();

                foreach (var other in candidates)
                {
                    if (combinator.TryCombine(_itemData.ItemID, other.ItemID, out ItemData result))
                    {
                        Debug.Log($"[Cheat 조합] {_itemData.ItemName} + {other.ItemName} → {result.ItemName}");

                        _itemData.IsComposited = true;
                        other.IsComposited = true;

                        _onLevelUp?.Invoke(_itemData);
                        _onLevelUp?.Invoke(other);

                        inventory.RemoveItem(_itemData);
                        inventory.RemoveItem(other);

                        inventory.AddItem(result);
                        result.ItemLevelUp();

                        _onLevelUp?.Invoke(result);
                        RefreshVisualState();
                        _inventoryUI.RenewUISlots();

                        return;
                    }
                }

                // 조합 불가 시 그냥 레벨업
                _itemData.ItemLevelUp();

                _onLevelUp?.Invoke(_itemData);
                RefreshVisualState();
                _inventoryUI.RenewUISlots();
            }
            else
            {
                _itemData.ItemLevelUp();

                _onLevelUp?.Invoke(_itemData);
                RefreshVisualState();
                _inventoryUI.RenewUISlots();
            }

            _onLevelUp?.Invoke(_itemData);
            RefreshVisualState();
            _inventoryUI.RenewUISlots();
        }
    }
}
