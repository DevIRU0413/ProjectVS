using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;
using ChoiceDataClass = ProjectVS.Dialogue.ChoiceData.ChoiceData;

namespace ProjectVS.Dialogue.ChoiceDataParser
{
    public static class ChoiceDataParser
    {
        public static List<ChoiceDataClass> Parse(CsvTable table)
        {
            CsvReader.Read(table);

            List<ChoiceDataClass> list = new();

            int rowCount = table.Table.GetLength(0);

            for (int r = 3; r < rowCount; r++)
            {
                ChoiceDataClass data = new GameObject($"ChoiceData_{r}").AddComponent<ChoiceDataClass>();

                data.ID = TryParseInt(table.GetData(r, 0));
                data.NextDialogueID = TryParseInt(table.GetData(r, 4));
                data.ChoiceText1 = TryParseString(table.GetData(r, 2));
                data.ChoiceText2 = TryParseString(table.GetData(r, 3));

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
