using System;
using System.Collections.Generic;

using ProjectVS.Unit.Player;

namespace ProjectVS.Data
{
    [Serializable]
    public class PlayerData
    {
        public PlayerStats          Stats;                    // 플레이어 스탯 및 레벨/경험치/현재 HP 등 (레벨 비례 스탯 상승 포함)

        public List<ItemData>       InventoryItems;           // 플레이어가 소지 중인 아이템 목록

        public int                  CurrentStage;             // 현재 진행 중인 스테이지 층 수 (몬스터 스탯 증가에 사용됨)
        public int                  MonstersDefeated;         // 누적 몬스터 처치 수 (도전 과제, 보상 등 트리거에 사용 가능)

        public int                  Gold;                     // 소지 골드 (강화, 상점 구매 등 재화)
        public int                  Diamonds;                 // 소지 다이아 (프리미엄 재화, 재화 획득 시 계정 정보에 저장 필요)

        public HashSet<int>         ReadDialogeIDs;            // 플레이어가 이미 읽은 대사 ID 목록 (중복 방지용)

        public int                  CurrentAffinityExp;        // 현재 NPC 호감도 경험치
        public int                  CurrentAffinityLevel;      // 현재 NPC 호감도 레벨

        public HashSet<string>      AcquiredCostumeName;       // 플레이어가 획득한 코스튬 이름
        public string               WornCostumeName;           // NPC가 현재 착용 중인 코스튬 이름

        public int                  TotalKills;                 // 몬스터 처치 횟수

        public float                TotalPlayTime;              // 총 플레이 타임
        public int                  BattleSceneCount;           // 누적 배틀씬 카운트

    }
}
