using System.Collections.Generic;

using ProjectVS.Data;
using ProjectVS.Utils.CsvParseUtils;
using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;

using UnityEngine;

namespace ProjectVS.Item
{
    public class ItemDataParser : MonoBehaviour
    {
        public static List<ItemData> Parse(CsvTable table)
        {
            CsvReader.Read(table);
            List<ItemData> list = new();

            int rowCount = table.Table.GetLength(0);

            for (int r = 3; r < rowCount; r++)
            {
                ItemData data = new()
                {
                    ItemID = CsvParseUtils.TryParseInt(table.GetData(r, 1)),
                    ItemRank = (ItemRank)CsvParseUtils.TryParseInt(table.GetData(r, 2)),
                    ItemName = CsvParseUtils.TryParseString(table.GetData(r, 3)),
                    ItemType = (ItemType)CsvParseUtils.TryParseInt(table.GetData(r, 4)),
                    AttackSpeed = CsvParseUtils.TryParseFloat(table.GetData(r, 5)),
                    ItemEffect = (ItemEffect)CsvParseUtils.TryParseInt(table.GetData(r, 6)),
                    ItemEffectValue = CsvParseUtils.TryParseFloat(table.GetData(r, 7)),
                    CombineItemID1 = CsvParseUtils.TryParseInt(table.GetData(r, 8)),
                    CombineItemID2 = CsvParseUtils.TryParseInt(table.GetData(r, 9)),
                    SetEffect = (ItemSetEffect)CsvParseUtils.TryParseInt(table.GetData(r, 10)),
                    SetEffectRequiredCount = CsvParseUtils.TryParseInt(table.GetData(r, 11)),
                    MaxLevel = CsvParseUtils.TryParseInt(table.GetData(r, 12)),
                    Price = CsvParseUtils.TryParseInt(table.GetData(r, 13)),
                    Range = CsvParseUtils.TryParseFloat(table.GetData(r, 14)),
                    Size = CsvParseUtils.TryParseFloat(table.GetData(r, 15)),
                    Description = CsvParseUtils.TryParseString(table.GetData(r, 16))
                    // ItemIconPath = CsvParseUtils.TryParseString(table.GetData(r, 17)) // TODO: 아이템 아이콘 경로(Resource폴더에 넣을) 추가해야 됨
                };
                list.Add(data);
            }

            return list;
        }
    }
}
