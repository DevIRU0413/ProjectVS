using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.CsvParseUtils;
using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;
using SetItemDataClass = ProjectVS.Item.SetItemData.SetItemData;

namespace ProjectVS.Item.SetItemDataParser
{
    public static class SetItemDataParser
    {
        public static List<SetItemDataClass> Parse(CsvTable table)
        {
            CsvReader.Read(table);
            List<SetItemDataClass> list = new();

            int rowCount = table.Table.GetLength(0);

            for (int r = 3; r < rowCount; r++)
            {
                SetItemDataClass data = new()
                {
                    ID = CsvParseUtils.TryParseInt(table.GetData(r, 0)),
                    Name = CsvParseUtils.TryParseString(table.GetData(r, 1)),
                    SetEffect = CsvParseUtils.TryParseInt(table.GetData(r, 2)),
                    SetEffectValue = CsvParseUtils.TryParseInt(table.GetData(r, 3)),
                    //BaseItemID
                    IllustPath = CsvParseUtils.TryParseString(table.GetData(r, 5))
                };

                list.Add(data);
            }

            return list;
        }
    }
}

