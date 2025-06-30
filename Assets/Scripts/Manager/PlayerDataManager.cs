using System;
using System.Collections.Generic;
using System.IO;

using ProjectVS.Data;
using ProjectVS.Interface;
using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Manager
{
    /// <summary>
    /// 저장된 정보들로 저장 데이터를 만듭니다.
    /// </summary>
    public class PlayerDataManager : SimpleSingleton<PlayerDataManager>, IGamePlayTypeListener
    {
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

        [Header("Dialogue")]
        public HashSet<int> ReadDialogeIDs;

        [Header("Affinity")]
        public int CurrentAffinityExp;
        public int CurrentAffinityLevel;

        [Header("NPC Costume")]
        public HashSet<string> AcquiredCostumeName;
        public string WornCostumeName;

        public void SavePlayerData()
        {
            PlayerData data = new PlayerData();

            data.Stats = stats;

            data.InventoryItems = inventoryItems;

            data.CurrentStage = currentStageFloor;
            data.MonstersDefeated = monstersDefeated;

            data.Gold = gold;
            data.Diamonds = diamonds;

            data.ReadDialogeIDs = ReadDialogeIDs;

            data.CurrentAffinityExp = CurrentAffinityExp;
            data.CurrentAffinityLevel = CurrentAffinityLevel;

            data.AcquiredCostumeName = AcquiredCostumeName;
            data.WornCostumeName = WornCostumeName;

            // Save
            SaveSystem.Save(data);
            print("저장");
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

            ReadDialogeIDs = data.ReadDialogeIDs;

            CurrentAffinityExp = data.CurrentAffinityExp;
            CurrentAffinityLevel = data.CurrentAffinityLevel;

            AcquiredCostumeName = data.AcquiredCostumeName;
            WornCostumeName = data.WornCostumeName;

            print("불러오기");
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

