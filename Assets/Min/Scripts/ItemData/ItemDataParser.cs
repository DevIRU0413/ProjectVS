using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.CsvReader;
using ItemDataClass = ProjectVS.ItemData.ItemData.ItemData;
using ProjectVS.Utils.CsvTable;
using ProjectVS.Utils.CsvParseUtils;


namespace ProjectVS.ItemData.ItemDataParser
{
    public class ItemDataParser : MonoBehaviour
    {
        public static List<ItemDataClass> Parse(CsvTable table)
        {
            CsvReader.Read(table);
            List<ItemDataClass> list = new();

            int rowCount = table.Table.GetLength(0);

            for (int r = 3; r < rowCount; r++)
            {
                ItemDataClass data = new()
                {
                    ID = CsvParseUtils.TryParseInt(table.GetData(r, 1)),
                    ItemRank = CsvParseUtils.TryParseString(table.GetData(r, 2)),
                    ItemName = CsvParseUtils.TryParseString(table.GetData(r, 3)),
                    ItemType = CsvParseUtils.TryParseString(table.GetData(r, 4)),
                    AttackSpeed = CsvParseUtils.TryParseFloat(table.GetData(r, 5)),
                    Effect = CsvParseUtils.TryParseString(table.GetData(r, 6)),
                    EffectValue = CsvParseUtils.TryParseFloat(table.GetData(r, 7)),
                    CombineItem1 = CsvParseUtils.TryParseInt(table.GetData(r, 8)),
                    CombineItem2 = CsvParseUtils.TryParseInt(table.GetData(r, 9)),
                    SetEffect = CsvParseUtils.TryParseString(table.GetData(r, 10)),
                    SetItemCount = CsvParseUtils.TryParseInt(table.GetData(r, 11)),
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
