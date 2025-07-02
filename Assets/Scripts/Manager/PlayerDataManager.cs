using System.Collections.Generic;

using ProjectVS.Data;
using ProjectVS.Data.Player;
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
        public GamePlayType gamePlayType;

        [Header("Test Play Config")]
        public CharacterClass testCharacterClass;

        [Header("Player Status")]
        public PlayerStats stats;

        [Header("Inventory & Items")]
        public List<ItemDataSO> inventoryItems = new List<ItemDataSO>();

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

        [Header("Misc")]
        public bool isMoodShifted;

        // IManager
        public int Priority => (int)ManagerPriority.PlayerDataManager;
        public bool IsDontDestroy => IsDontDestroyOnLoad;
        public GameObject GetGameObject() => this.gameObject;
        public void Cleanup() { }
        public void Initialize()
        {
            gamePlayType = GameManager.Instance.GamePlayType;
            if (gamePlayType == GamePlayType.Test)
            {
                stats = new();
                stats = stats.TestStats(CharacterClass.Axe);
            }
        }

        // Manager Func
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

