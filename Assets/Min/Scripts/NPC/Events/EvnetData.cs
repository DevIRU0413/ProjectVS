using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace ProjectVS.NPC.Events.EvnetData
{
    public enum EventType
    {
        Once = 1,           // 해당 이벤트의 보상은 해당 이벤트가 발생했을 때 한 번 작동한다
        Duration = 2        // 해당 이벤트의 보상은 게임 오버 시 까지 지속된다
    }

    public enum EventOccurTiming
    {
        FieldRescue = 1,    // 필드 내에서 '속박된 상점 NPC' 구출 선택 시 상점에서 이벤트 발동
        FieldExtort = 2,    // 필드 내에서 '속박된 상점 NPC' 갈취 선택 시 필드에서 즉시 이벤트 발동
        Shop = 3            // 상점 내에서 조건 충족 시 이벤트 발동
    }
    public enum AffinityModifyType
    {
        No = 0,             // 호감도 증감 없음
        Increase = 1,       // 호감도 수치가 증가한다
        Decrease = 2        // 호감도 수치가 감소한다
    }

    public enum EventReward
    {
        RandomItem = 1,     // 랜덤 아이템 지급 (최대 강화 수치 아이템 제외), 상점 도달 시 무작위 아이템을 지급한다. (플레이어 소지 최대 강화 수치 아이템 제외)
        DropMoney = 2,      // 무작위 재화 드랍 (다이아, 골드), 무작위 재화(다이아, 골드)를 드랍한다. (수치 미정)
        DiscountAll = 3,    // 모든 아이템 영구 할인, 모든 아이템을 영구적으로 3% 할인한다
        DiscountOnce = 4    // 아이템 하나 한 번 (상점 방문 1회) 할인, 다음 상점 방문 시 아이템 하나를 10%(수치 미정) 할인한다. (할인 후 다음 상점 도달 시 초기화)
    }



    [System.Serializable]
    public class EvnetData
    {
        public int EventID;
        public EventType EventType;
        public EventOccurTiming EventOccurTiming;
        public EventReward EventReward;
        public int EventOccurAffinity;
        public AffinityModifyType AffinityModifyType;
        public int EventRewardAffinity;
    }
}
