using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace ProjectVS.Manager
{
    [System.Serializable]
    public class Item
    {
        public string itemName;
        public int itemID;
    }

    [System.Serializable]
    public class NPC
    {
        public string npcName;
        public int npcID;
    }

    [Serializable]
    public class ItemUpgradeData
    {
        public string itemName;
        public int upgradeLevel;
    }

    [Serializable]
    public class AffectionData
    {
        public string npcName;
        public int affectionLevel;
    }

    [Serializable]
    public class NpcCostumeData
    {
        public string npcName;
        public string costumeId;
    }

    public class PlayerDataManager : MonoBehaviour
    {
        [Header("Player Status")]
        public int playerLevel;
        public int playerExp;
        public int currentHP;

        [Header("Inventory & Items")]
        public List<Item> inventoryItems = new List<Item>();
        public Dictionary<Item, int> itemUpgradeLevels = new Dictionary<Item, int>();

        [Header("Progress Info")]
        public int currentStageFloor;
        public int monstersDefeated;

        [Header("Currency")]
        public int gold;
        public int diamonds;

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

        public void SavePlayerData()
        {
            PlayerData data = new PlayerData();

            data.playerLevel = playerLevel;
            data.playerExp = playerExp;
            data.currentHP = currentHP;
            data.inventoryItems = inventoryItems;

            data.itemUpgrades = new List<ItemUpgradeData>();
            foreach (var pair in itemUpgradeLevels)
            {
                data.itemUpgrades.Add(new ItemUpgradeData { itemName = pair.Key.itemName, upgradeLevel = pair.Value });
            }

            data.currentStageFloor = currentStageFloor;
            data.monstersDefeated = monstersDefeated;

            data.gold = gold;
            data.diamonds = diamonds;

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

            SaveSystem.Save(data);
        }

        public void LoadPlayerData()
        {
            PlayerData data = SaveSystem.Load();
            if (data == null) return;

            playerLevel = data.playerLevel;
            playerExp = data.playerExp;
            currentHP = data.currentHP;
            inventoryItems = data.inventoryItems;

            itemUpgradeLevels.Clear();
            foreach (var upgrade in data.itemUpgrades)
            {
                var item = inventoryItems.Find(i => i.itemName == upgrade.itemName);
                if (item != null)
                    itemUpgradeLevels[item] = upgrade.upgradeLevel;
            }

            currentStageFloor = data.currentStageFloor;
            monstersDefeated = data.monstersDefeated;

            gold = data.gold;
            diamonds = data.diamonds;

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
    }

    public static class SaveSystem
    {
        private static readonly string savePath = Path.Combine(Application.persistentDataPath, "playerdata.json");

        public static void Save(PlayerData data)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(savePath, json);
            Debug.Log("Game Saved to: " + savePath);
        }

        public static PlayerData Load()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                return JsonUtility.FromJson<PlayerData>(json);
            }
            else
            {
                Debug.LogWarning("Save file not found");
                return null;
            }
        }
    }

    [Serializable]
    public class PlayerData
    {
        public int playerLevel;
        public int playerExp;
        public int currentHP;

        public List<Item> inventoryItems;
        public List<ItemUpgradeData> itemUpgrades;

        public int currentStageFloor;
        public int monstersDefeated;

        public int gold;
        public int diamonds;

        public List<AffectionData> affectionLevels;
        public List<string> affectionEventFlags;

        public List<string> dialogueProgressFlags;

        public List<string> ownedCostumes;
        public List<NpcCostumeData> npcCostumes;

        public bool isMoodShifted;
    }

}

