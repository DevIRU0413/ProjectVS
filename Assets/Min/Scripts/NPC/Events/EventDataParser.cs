using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.NPC.Events.EvnetData;
using ProjectVS.Utils.CsvParseUtils;
using ProjectVS.Utils.CsvReader;
using ProjectVS.Utils.CsvTable;

using EventType = ProjectVS.NPC.Events.EvnetData.EventType;
using EventDataClass = ProjectVS.NPC.Events.EvnetData.EvnetData;



namespace ProjectVS.NPC.Events.EventDataParser
{
    public static class EventDataParser
    {
        public static List<EventDataClass> Parse(CsvTable table)
        {
            CsvReader.Read(table);
            List<EventDataClass> list = new();

            int rowCount = table.Table.GetLength(0);

            for (int r = 3; r < rowCount; r++)
            {
                EventDataClass data = new()
                {
                    EventID = CsvParseUtils.TryParseInt(table.GetData(r, 1)),
                    EventType = (EventType)CsvParseUtils.TryParseInt(table.GetData(r, 2)),
                    EventOccurTiming = (EventOccurTiming)CsvParseUtils.TryParseInt(table.GetData(r, 3)),
                    EventReward = (EventReward)CsvParseUtils.TryParseInt(table.GetData(r, 4)),
                    EventOccurAffinity = CsvParseUtils.TryParseInt(table.GetData(r, 5)),
                    AffinityModifyType = (AffinityModifyType)CsvParseUtils.TryParseInt(table.GetData(r, 6)),
                    EventRewardAffinity = CsvParseUtils.TryParseInt(table.GetData(r, 7))
                };

                list.Add(data);
            }

            return list;
        }
    }
}
