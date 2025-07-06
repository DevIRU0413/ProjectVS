using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.CsvTable;
using ProjectVS.Utils.CsvReader;
using DialogueDataClass = ProjectVS.Dialogue.DialogueData.DialogueData;
using ProjectVS.Utils.CsvParseUtils;

namespace ProjectVS.Unit.Monster
{
    public static class MonsterDataParser
    {
        public static List<DialogueDataClass> Parse(CsvTable table)
        {
            CsvReader.Read(table);
            List<DialogueDataClass> list = new();

            int rowCount = table.Table.GetLength(0);

            for (int r = 3; r < rowCount; r++)
            {
                DialogueDataClass data = new()
                {
                    ID = CsvParseUtils.TryParseInt(table.GetData(r, 0)),
                    CharacterID = CsvParseUtils.TryParseInt(table.GetData(r, 1)),
                    NeedAffinity = CsvParseUtils.TryParseInt(table.GetData(r, 2)),
                    IllustPath = CsvParseUtils.TryParseString(table.GetData(r, 3)),
                    Content = CsvParseUtils.TryParseString(table.GetData(r, 4)),
                    ContextNote = CsvParseUtils.TryParseString(table.GetData(r, 6)),
                    OccurTiming = CsvParseUtils.TryParseInt(table.GetData(r, 7))
                };

                list.Add(data);
            }

            return list;
        }
    }
}
