using System.Collections.Generic;

using ProjectVS.Data;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Item
{
    public class ItemDatabase : SimpleSingleton<ItemDatabase>
    {
        [SerializeField] private List<ItemData> itemDataList = new();
        private Dictionary<int, ItemData> _itemDict = new();

        protected override void Awake()
        {
            InitDatabase();
        }

        // 초기 등록용
        private void InitDatabase()
        {
            foreach (var item in itemDataList)
            {
                if (!_itemDict.ContainsKey(item.ItemID))
                    _itemDict.Add(item.ItemID, item);
                else
                    Debug.LogWarning($"[ItemDatabase] 중복된 아이템 ID: {item.ItemID}");
            }
        }

        /// <summary>
        /// 아이템 조회용
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ItemData GetItem(int id)
        {
            _itemDict.TryGetValue(id, out var item);
            return item;
        }

        /// <summary>
        /// 아이템 리스트 전체 확인용
        /// </summary>
        /// <returns></returns>
        public List<ItemData> GetAllItems() => itemDataList;
    }
}
