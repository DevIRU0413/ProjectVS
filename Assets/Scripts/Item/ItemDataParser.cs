using System.Collections.Generic;

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

            for (int r = 4; r < rowCount; r++)
            {
                ItemData data = new()
                {
                    ItemID = CsvParseUtils.TryParseInt(table.GetData(r, 1)),
                    ItemRank = (ItemRank)CsvParseUtils.TryParseInt(table.GetData(r, 2)),
                    ItemName = CsvParseUtils.TryParseString(table.GetData(r, 3)),
                    ItemType = (ItemType)CsvParseUtils.TryParseInt(table.GetData(r, 4)),
                    ItemAtkSpeed = CsvParseUtils.TryParseFloat(table.GetData(r, 5)),
                    ItemEffect = (ItemEffect)CsvParseUtils.TryParseInt(table.GetData(r, 6)),
                    ItemEffectValue = CsvParseUtils.TryParseInt(table.GetData(r, 7)),
                    ItemAddID1 = CsvParseUtils.TryParseInt(table.GetData(r, 8)),
                    ItemAddID2 = CsvParseUtils.TryParseInt(table.GetData(r, 9)),
                    ItemSetEffect = (ItemSetEffect)CsvParseUtils.TryParseInt(table.GetData(r, 10)),
                    ItemSetNum = CsvParseUtils.TryParseInt(table.GetData(r, 11)),
                    ItemMaxLevel = CsvParseUtils.TryParseInt(table.GetData(r, 12)),
                    Price = CsvParseUtils.TryParseInt(table.GetData(r, 13)),
                    ItemRange = CsvParseUtils.TryParseInt(table.GetData(r, 14)),
                    ItemSize = CsvParseUtils.TryParseFloat(table.GetData(r, 15)),
                    ItemIconPath = CsvParseUtils.TryParseString(table.GetData(r, 16)),
                    Description = CsvParseUtils.TryParseString(table.GetData(r, 17))
                };
                list.Add(data);
            }

            return list;
        }
    }
}
