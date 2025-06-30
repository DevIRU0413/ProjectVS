using System;
using System.Collections.Generic;
using System.IO;

using ProjectVS.Data;
using ProjectVS.Interface;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Manager
{
    // 임시
    [Serializable]
    public class NPC
    {
        public string npcName;
        public int npcID;
    }

    // 임시
    [Serializable]
    public class AffectionData
    {
        public string npcName;
        public int affectionLevel;
    }

    // 임시
    [Serializable]
    public class NpcCostumeData
    {
        public string npcName;
        public string costumeId;
    }

    /// <summary>
    /// 저장된 정보들로 저장 데이터를 만듭니다.
    /// </summary>
    public class PlayerDataManager : SimpleSingleton<PlayerDataManager>, IGamePlayTypeListener
    {
        [HideInInspector]
        public GamePlayType GamePlayType;

        [Header("Test Play Config")]
        public CharacterClass TestCharacterClass;

        [Header("Player Status")]
        public PlayerStats stats;

        [Header("Inventory & Items")]
        public List<Item> inventoryItems = new List<Item>();

        [Header("Progress Info")]
        public int currentStageFloor;
        public int monstersDefeated;

        [Header("Currency")]
        public int gold;
        public int diamonds;

        #region Help Me... R.I.P(수정하고 지워 주세요)
        [Header("Affection System")]
        public Dictionary<NPC, int> affectionLevels = new Dictionary<NPC, int>();
        public HashSet<string> affectionEventFlags = new HashSet<string>(); // 이벤트 ID

        [Header("Dialogue Tracking")]
        public HashSet<string> dialogueProgressFlags = new HashSet<string>(); // 대사 ID

        [Header("Costumes")]
        public HashSet<string> ownedCostumes = new HashSet<string>(); // 코스튬 ID
        public Dictionary<NPC, string> npcCostumeMap = new Dictionary<NPC, string>(); // NPC -> 착용 코스튬

        [Header("Misc")]
        public bool isMoodShifted;
        #endregion

        public void SavePlayerData()
        {
            PlayerData data = new PlayerData();

            data.Stats = stats;

            data.InventoryItems = inventoryItems;

            data.CurrentStage = currentStageFloor;
            data.MonstersDefeated = monstersDefeated;

            data.Gold = gold;
            data.Diamonds = diamonds;

            // Help Me... R.I.P(수정하고 지워 주세요)
            data.affectionLevels = new List<AffectionData>();
            foreach (var pair in affectionLevels)
            {
                data.affectionLevels.Add(new AffectionData { npcName = pair.Key.npcName, affectionLevel = pair.Value });
            }

            data.affectionEventFlags = new List<string>(affectionEventFlags);
            data.dialogueProgressFlags = new List<string>(dialogueProgressFlags);
            data.ownedCostumes = new List<string>(ownedCostumes);

            data.npcCostumes = new List<NpcCostumeData>();
            foreach (var pair in npcCostumeMap)
            {
                data.npcCostumes.Add(new NpcCostumeData { npcName = pair.Key.npcName, costumeId = pair.Value });
            }

            data.isMoodShifted = isMoodShifted;

            // Save
            SaveSystem.Save(data);
        }

        public void LoadPlayerData()
        {
            // Load
            PlayerData data = SaveSystem.Load();
            if (data == null) return;

            inventoryItems = data.InventoryItems;

            currentStageFloor = data.CurrentStage;
            monstersDefeated = data.MonstersDefeated;

            gold = data.Gold;
            diamonds = data.Diamonds;

            affectionLevels.Clear();
            foreach (var aff in data.affectionLevels)
            {
                affectionLevels[new NPC { npcName = aff.npcName }] = aff.affectionLevel;
            }

            affectionEventFlags = new HashSet<string>(data.affectionEventFlags);
            dialogueProgressFlags = new HashSet<string>(data.dialogueProgressFlags);
            ownedCostumes = new HashSet<string>(data.ownedCostumes);

            npcCostumeMap.Clear();
            foreach (var npcCostume in data.npcCostumes)
            {
                npcCostumeMap[new NPC { npcName = npcCostume.npcName }] = npcCostume.costumeId;
            }

            isMoodShifted = data.isMoodShifted;
        }

        public void OnGamePlayTypeChanged(GamePlayType type)
        {
            GamePlayType = type;
            if (type == GamePlayType.Build) return;

            if (TestCharacterClass == CharacterClass.None)
                TestCharacterClass = CharacterClass.Sword;

            stats = new PlayerStats();
            stats = stats.TestStats(TestCharacterClass);
        }
    }
}

