using ProjectVS.Manager;
using System.Collections.Generic;
using System;

namespace ProjectVS.Data
{
    [Serializable]
    public class PlayerData
    {
        public PlayerStats                    Stats;                    // 플레이어 스탯 및 레벨/경험치/현재 HP 등 (레벨 비례 스탯 상승 포함)

        public List<Item>                     InventoryItems;           // 플레이어가 소지 중인 아이템 목록

        public int                            CurrentStage;             // 현재 진행 중인 스테이지 층 수 (몬스터 스탯 증가에 사용됨)
        public int                            MonstersDefeated;         // 누적 몬스터 처치 수 (도전 과제, 보상 등 트리거에 사용 가능)

        public int                            Gold;                     // 소지 골드 (강화, 상점 구매 등 재화)
        public int                            Diamonds;                 // 소지 다이아 (프리미엄 재화, 재화 획득 시 계정 정보에 저장 필요)

        // Help Me... R.I.P(수정하고 지워 주세요)
        public List<AffectionData>           affectionLevels;          // 각 NPC 별 호감도 수치 및 경험치 (레벨 및 경험치 포함)
        public List<string>                  affectionEventFlags;      // 이미 본 호감도 이벤트 플래그 (중복 발생 방지용)

        public List<string>                  dialogueProgressFlags;    // 대사 진행 플래그 (호감도 이벤트 흐름 제어)

        public List<string>                  ownedCostumes;            // 플레이어가 보유한 코스튬 목록 (재구매 방지 목적)
        public List<NpcCostumeData>          npcCostumes;              // 각 NPC가 현재 착용 중인 코스튬 정보

        public bool                          isMoodShifted;            // 플레이어 감정 변화 상태 여부 (특정 조건에서 모드 전환 여부 판단)
    }
}
