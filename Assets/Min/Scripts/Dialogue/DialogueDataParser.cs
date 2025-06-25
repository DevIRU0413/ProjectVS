using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.CsvTable;
using ProjectVS.Utils.CsvReader;
using DialogueDataClass = ProjectVS.Dialogue.DialogueData.DialogueData;


namespace ProjectVS.Dialogue.DialogueDataParser
{
    public static class DialogueDataParser
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
                    ID = TryParseInt(table.GetData(r, 0)),
                    CharacterID = TryParseInt(table.GetData(r, 1)),
                    NeedAffinity = TryParseInt(table.GetData(r, 2)),
                    IllustPath = TryParseString(table.GetData(r, 3)),
                    Content = TryParseString(table.GetData(r, 4)),
                    ContextNote = TryParseString(table.GetData(r, 6)),
                    OccurTiming = TryParseInt(table.GetData(r, 7))
                };

                list.Add(data);
            }

            return list;
        }

        private static string TryParseString(string raw)
        {
            return string.IsNullOrEmpty(raw) ? "N/A" : raw;
        }

        private static int TryParseInt(string raw)
        {
            return int.TryParse(raw, out int result) ? result : -1;
        }
    }
}
