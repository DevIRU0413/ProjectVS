using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;
using CharacterSelectionDataClass = ProjectVS.CharacterSelectionData.CharacterSelectionData.CharacterSelectionData;
using ProjectVS.Utils.CsvParseUtils;


namespace ProjectVS.CharacterSelectionData.CharacterSelectionDataParser
{
    public static class CharacterSelectionDataParser
    {
        public static List<CharacterSelectionDataClass> Parse(CsvTable table)
        {
            CsvReader.Read(table);
            List<CharacterSelectionDataClass> list = new();

            int rowCount = table.Table.GetLength(0);

            for (int r = 4; r < rowCount; r++)
            {
                CharacterSelectionDataClass data = new()
                {
                    ID = CsvParseUtils.TryParseInt(table.GetData(r, 1)),
                    Name = CsvParseUtils.TryParseString(table.GetData(r, 2)),
                    UniqueItemID = CsvParseUtils.TryParseInt(table.GetData(r, 3)),
                    Attack = CsvParseUtils.TryParseInt(table.GetData(r, 4)),
                    Defense = CsvParseUtils.TryParseInt(table.GetData(r, 5)),
                    HP = CsvParseUtils.TryParseInt(table.GetData(r, 6)),
                    AttackSpeed = CsvParseUtils.TryParseInt(table.GetData(r, 7)),
                    MoveSpeed = CsvParseUtils.TryParseInt(table.GetData(r, 8)),
                    IllustPath = CsvParseUtils.TryParseString(table.GetData(r, 9)),
                    FlavorText = CsvParseUtils.TryParseString(table.GetData(r, 10))
                };

                list.Add(data);
            }

            return list;
        }
    }
}
