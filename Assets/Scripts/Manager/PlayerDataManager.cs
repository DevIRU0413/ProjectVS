using System.Collections.Generic;

using ProjectVS.Data;
using ProjectVS.Interface;
using ProjectVS.Unit.Player;
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
        public PlayerStats Stats;

        [Header("Inventory & Items")]
        public List<ItemData> InventoryItems = new List<ItemData>();

        [Header("Progress Info")]
        public int CurrentStageFloor;
        public int MonstersDefeated;

        [Header("Currency")]
        public int Gold;
        public int Diamonds;

        [Header("Dialogue")]
        public HashSet<int> ReadDialogeIDs;

        [Header("Affinity")]
        public int CurrentAffinityExp;
        public int CurrentAffinityLevel;

        [Header("NPC Costume")]
        public HashSet<string> AcquiredCostumeName;
        public string WornCostumeName;

        [Header("MonsterScore")]
        public int TotalKills; // 몬스터 총 처치 수

        [Header("Playtime Info")]
        public float TotalPlayTime; // 총 플레이 시간 (초 단위)
        public int BattleSceneCount; // 전투씬 진입 횟수

        

        public int Priority => (int)ManagerPriority.PlayerDataManager;
        public bool IsDontDestroy => IsDontDestroyOnLoad;
        public GameObject GetGameObject() => this.gameObject;
        protected override void Awake()
        {
            // 데이터 매니저는 게임 매니저의 컴포넌트로 들어가고 게임 매니저가 유지되어야 플레이어 데이터의 세이브/로드가 유지됨
            base.Awake();
            DontDestroyOnLoad(this.gameObject);
        }
        public void Initialize()
        {
            GamePlayType = GameManager.Instance.GamePlayType;
            if (GamePlayType == GamePlayType.Build) return;

            if (TestCharacterClass == CharacterClass.None)
                TestCharacterClass = CharacterClass.Sword;

            // stats = new PlayerStats(); // playerStats에서 playerConfig로 클래스 가져올 수 있도록 변경함
            // stats = stats.TestStats(TestCharacterClass);

         //  Stats = new Unit.Player.PlayerStats();
         //  Stats = Stats.TestStats(TestCharacterClass);
        }
        public void Cleanup() { }


        public void SavePlayerData(int index)
        {
            PlayerData data = new PlayerData();
        
            data.Stats = Stats;

            data.InventoryItems = InventoryItems;

            data.CurrentStage = CurrentStageFloor;
            data.MonstersDefeated = MonstersDefeated;

            data.Gold = Gold;
            data.Diamonds = Diamonds;

            data.ReadDialogeIDs = ReadDialogeIDs;

            data.CurrentAffinityExp = CurrentAffinityExp;
            data.CurrentAffinityLevel = CurrentAffinityLevel;

            data.AcquiredCostumeName = AcquiredCostumeName;
            data.WornCostumeName = WornCostumeName;

            data.TotalKills = TotalKills;

            data.TotalPlayTime = TotalPlayTime;
            data.BattleSceneCount = BattleSceneCount;

            Debug.Log($"[저장 직전] HP: {Stats.CurrentHp}, Atk: {Stats.CurrentAtk}, 레벨: {Stats.Level}, Exp: {Stats.CurrentExp}");


            // Save
            SaveFileSystem.Save(data, index);
            print("저장");

        }

        public void LoadPlayerData(int index)
        {
            // Load
            PlayerData data = SaveFileSystem.Load(index);
            if (data == null) return;

            InventoryItems = data.InventoryItems;

            CurrentStageFloor = data.CurrentStage;
            MonstersDefeated = data.MonstersDefeated;

            Gold = data.Gold;
            Diamonds = data.Diamonds;

            ReadDialogeIDs = data.ReadDialogeIDs;

            CurrentAffinityExp = data.CurrentAffinityExp;
            CurrentAffinityLevel = data.CurrentAffinityLevel;

            AcquiredCostumeName = data.AcquiredCostumeName;
            WornCostumeName = data.WornCostumeName;

            TotalKills = data.TotalKills;

            TotalPlayTime = data.TotalPlayTime;
            BattleSceneCount = data.BattleSceneCount;

            print("불러오기");

            Stats = new PlayerStats();// 비어 있는 Stats 객체 생성
            Stats.ApplyFrom(data.Stats);// 저장된 데이터로 덮어쓰기
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

