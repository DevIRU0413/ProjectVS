using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;
using ChoiceDataClass = ProjectVS.Dialogue.ChoiceData.ChoiceData;
using ProjectVS.Utils.CsvParseUtils;


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
                ChoiceDataClass data = new();

                data.ID = CsvParseUtils.TryParseInt(table.GetData(r, 0));
                data.NextDialogueID = CsvParseUtils.TryParseInt(table.GetData(r, 4));
                data.ChoiceText1 = CsvParseUtils.TryParseString(table.GetData(r, 2));
                data.ChoiceText2 = CsvParseUtils.TryParseString(table.GetData(r, 3));

                list.Add(data);
            }

            return list;
        }
    }
}
