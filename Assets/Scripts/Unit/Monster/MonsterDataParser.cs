using System;
using System.Collections.Generic;

using ProjectVS.Utils.CsvParseUtils;
using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;

namespace ProjectVS.Unit.Monster
{
    public static class MonsterDataParser
    {
        public static List<MonsterStatsConfig> Parse(CsvTable table)
        {
            CsvReader.Read(table); // 필요한 경우만 호출

            List<MonsterStatsConfig> list = new();

            int rowCount = table.Table.GetLength(0);

            for (int r = 4; r < rowCount; r++) // 헤더+타입 3줄 넘기고
            {
                MonsterClass parsedClass;
                Enum.TryParse(CsvParseUtils.TryParseString(table.GetData(r, 6)), out parsedClass);
                var data = new MonsterStatsConfig
                {
                    ID = CsvParseUtils.TryParseInt(table.GetData(r, 1)),
                    MonsterName = CsvParseUtils.TryParseString(table.GetData(r, 2)),
                    SizeValue = CsvParseUtils.TryParseInt(table.GetData(r, 5)),
                    Class = parsedClass,

                    Hp = CsvParseUtils.TryParseInt(table.GetData(r, 9)),
                    ATK = CsvParseUtils.TryParseInt(table.GetData(r, 10)),
                    ATKSPD = CsvParseUtils.TryParseFloat(table.GetData(r, 11)),
                    ATKRange = CsvParseUtils.TryParseFloat(table.GetData(r, 12)),

                    SPD = CsvParseUtils.TryParseFloat(table.GetData(r, 14)),
                    DFS = CsvParseUtils.TryParseInt(table.GetData(r, 15)),

                    Exp = CsvParseUtils.TryParseInt(table.GetData(r, 18)),
                    DropGold = CsvParseUtils.TryParseInt(table.GetData(r, 19)),
                    DropGoldPer = CsvParseUtils.TryParseInt(table.GetData(r, 20)),
                    DropDiamond = CsvParseUtils.TryParseInt(table.GetData(r, 21)),
                    DropDiamondPer = CsvParseUtils.TryParseInt(table.GetData(r, 22)),
                };

                list.Add(data);
            }

            return list;
        }
    }
}
