using System.Collections.Generic;
using System.Linq;

using ProjectVS.Data;

namespace ProjectVS.Item
{
    public class ItemInventory
    {
        private readonly List<ItemData> _inventoryItems = new();

        /// <summary>
        /// 아이템 추가
        /// </summary>
        public void AddItem(ItemData item)
        {
            if (item != null)
            {
                _inventoryItems.Add(item);
                EvaluateSetEffects(_inventoryItems);
            }
        }

        /// <summary>
        /// 아이템 제거
        /// </summary>
        public bool RemoveItem(ItemData item)
        {
            return _inventoryItems.Remove(item);
        }

        /// <summary>
        /// 특정 아이템을 보유 중인지 확인
        /// </summary>
        public bool HasItem(int itemId)
        {
            return _inventoryItems.Any(i => i.ItemID == itemId);
        }

        /// <summary>
        /// 특정 ID의 아이템 반환 (여러 개 있을 수 있음)
        /// </summary>
        public List<ItemData> GetItemsByID(int itemId)
        {
            return _inventoryItems.Where(i => i.ItemID == itemId).ToList();
        }

        /// <summary>
        /// 현재 인벤토리의 모든 아이템 반환
        /// </summary>
        public List<ItemData> GetAllItems()
        {
            return new List<ItemData>(_inventoryItems);
        }

        /// <summary>
        /// 전체 인벤토리 비우기
        /// </summary>
        public void ClearInventory()
        {
            _inventoryItems.Clear();
        }

        /// <summary>
        /// 현재 장착 아이템 리스트를 기준으로 발동된 세트 효과 목록 반환
        /// </summary>
        public static List<ItemSetEffect> EvaluateSetEffects(List<ItemData> equippedItems)
        {
            Dictionary<ItemSetEffect, List<ItemData>> setGroups = new();

            // 세트가 있는 아이템만 그룹핑
            foreach (var item in equippedItems)
            {
                if (item == null || item.SetEffect == ItemSetEffect.None)
                    continue;

                if (!setGroups.ContainsKey(item.SetEffect))
                    setGroups[item.SetEffect] = new List<ItemData>();

                setGroups[item.SetEffect].Add(item);
            }

            // 조건을 만족하는 세트만 추출
            List<ItemSetEffect> activatedSets = new();
            foreach (var kvp in setGroups)
            {
                var setEffect = kvp.Key;
                var items = kvp.Value;

                // 어떤 아이템이든 조건 만족하는 값만 있으면 발동
                int required = items[0].SetEffectRequiredCount;
                if (items.Count >= required)
                {
                    activatedSets.Add(setEffect);
                }
            }

            return activatedSets;
        }
    }
}

