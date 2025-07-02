using System.Collections.Generic;

using ProjectVS.Data;

namespace ProjectVS.Item
{
    public class ItemCombinator
    {
        private readonly Dictionary<(int, int), ItemData> _combineDict = new();

        public ItemCombinator(List<ItemData> allItems)
        {
            BuildCombinationMap(allItems);
        }

        private void BuildCombinationMap(List<ItemData> items)
        {
            foreach (var item in items)
            {
                if (item.CombineItemID1 <= 0 || item.CombineItemID2 <= 0)
                    continue;

                // 두 ID를 정렬해서 키 생성 (순서 무관)
                int a = item.CombineItemID1;
                int b = item.CombineItemID2;
                var key = CreateKey(a, b);

                if (!_combineDict.ContainsKey(key))
                    _combineDict.Add(key, item);
            }
        }

        private (int, int) CreateKey(int id1, int id2)
        {
            return id1 < id2 ? (id1, id2) : (id2, id1);
        }

        /// <summary>
        /// 두 아이템 ID로 조합 가능한 아이템 반환
        /// </summary>
        public bool TryCombine(int id1, int id2, out ItemData result)
        {
            return _combineDict.TryGetValue(CreateKey(id1, id2), out result);
        }
    }
}
