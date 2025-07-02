using System.Collections.Generic;

using ProjectVS.Data;
using ProjectVS.Interface;
using ProjectVS.Util;

using UnityEngine;
using UnityEngine.Rendering;

namespace ProjectVS.Manager
{
    /// <summary>
    /// 저장된 정보들로 저장 데이터를 만듭니다.
    /// </summary>
    public class PlayerDataManager : SimpleSingleton<PlayerDataManager>, IManager
    {
        public GamePlayType GamePlayType;

        [Header("Test Play Config")]
        public CharacterClass TestCharacterClass;

        [Header("Player Status")]
        public Unit.Player.PlayerStats stats;

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

        [Header("MonsterScore")]
        public int totalKills; // 몬스터 총 처치 수

        [Header("Playtime Info")]
        public float totalPlayTime; // 총 플레이 시간 (초 단위)
        public int battleSceneCount; // 전투씬 진입 횟수

        public int Priority => (int)ManagerPriority.PlayerDataManager;
        public bool IsDontDestroy => IsDontDestroyOnLoad;
        public GameObject GetGameObject() => this.gameObject;
     
        public void Initialize()
        {
            GamePlayType = GameManager.Instance.GamePlayType;
            if (GamePlayType == GamePlayType.Build) return;

            if (TestCharacterClass == CharacterClass.None)
                TestCharacterClass = CharacterClass.Sword;

            // stats = new PlayerStats(); // playerStats에서 playerConfig로 클래스 가져올 수 있도록 변경함
            // stats = stats.TestStats(TestCharacterClass);

            stats = new Unit.Player.PlayerStats();
            stats = stats.TestStats(TestCharacterClass);
        }
        public void Cleanup() { }


        public void SavePlayerData(int index)
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

            data.TotalKills = totalKills;

            data.TotalPlayTime = totalPlayTime;
            data.BattleSceneCount = battleSceneCount;

            // Save
            SaveFileSystem.Save(data, index);
            print("저장");
        }

        public void LoadPlayerData(int index)
        {
            // Load
            PlayerData data = SaveFileSystem.Load(index);
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

            totalKills = data.TotalKills;

            totalPlayTime = data.TotalPlayTime;
            battleSceneCount = data.BattleSceneCount;

            print("불러오기");
        }

        public void DeletePlayerData(int index)
        {
            SaveFileSystem.Delete(index);
        }

        public bool CheckPlayerData(int index)
        {
            return SaveFileSystem.HasSaveData(index);
        }
    }
}

