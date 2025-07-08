using System.Collections.Generic;
using System.Linq;

using ProjectVS.Interface;
using ProjectVS.Manager;
using ProjectVS.Util;
using ProjectVS.Utils.UIManager;

using UnityEngine;
using UnityEngine.InputSystem;

using BuyItemObjBehaviourClass = ProjectVS.Item.BuyItemObjBehaviour.BuyItemObjBehaviour;
using GetItemButtonBehaviourClass = ProjectVS.Item.GetItemButtonBehaviour.GetItemButtonBehaviour;
using InventoryUIClass = ProjectVS.UIs.Item.InventoryUI.InventoryUI;



namespace ProjectVS.Item.ItemManager
{
    /// <summary>
    /// 인벤토리 조작 및 인스턴스 유지 접근, 아이템 조합기 보유, UI 연결 역할 수행
    /// </summary>

    // TODO: 역할 분리 가능할 듯
    public class ItemManager : SimpleSingleton<ItemManager>, IManager
    {
        [SerializeField] private InventoryUIClass _inventoryUI;

        private List<ItemData> _itemPool = new();

        private ItemCombinator _itemCombinator;
        private ItemInventory _inventory;
        public ItemInventory Inventory => _inventory;

        [Header("아이템 획득 및 구매 연동")]
        [SerializeField] private List<GetItemButtonBehaviourClass> _buttonList = new();
        [SerializeField] private List<BuyItemObjBehaviourClass> _objList = new();

        public int Priority => (int)ManagerPriority.ItemManager;
        public bool IsDontDestroy => IsDontDestroyOnLoad;

        //public event Action OnInventoryChanged;

        protected override void Awake()
        {
            base.Awake();

            if (_itemCombinator == null)
                _itemCombinator = new(ItemDatabase.Instance.GetAllItems());
        }

        private void Update()
        {
            if (Keyboard.current.uKey.wasPressedThisFrame)
            {
                LevelUpItem();
            }
        }

        private void Start()
        {
            if (_objList.Count > 0)
                DisplayShopItem();

            if (GameManager.Instance.GamePlayType == GamePlayType.Test)
            {
                // TestInitUniqueItem();
            }
        }


        /// <summary>
        /// 레벨 업 시 호출
        /// </summary>
        [ContextMenu("Test Level Up")]
        public void LevelUpItem()
        {
            Debug.Log($"[ItemManager] 레벨 업 호출됨");

            List<ItemData> levelUpPool = ReturnItemForLvlUp(3);

            // 레벨업 할 것이 하나도 없다면
            if (levelUpPool.Count == 0)
            {
                Debug.Log("[ItemManager] 레벨업 할 것이 아무것도 없어 return 합니다");
                return;
            }

            for (int i = 0; i < 3; i++)
            {
                if (i < levelUpPool.Count)
                {
                    _buttonList[i].Init(levelUpPool[i], _itemCombinator, _inventory);
                }
                else
                {
                    _buttonList[i].Init(null, null, null); // 아이템이 부족한 경우 표시 
                }
            }

            UIManager.Instance.Show("Level Up Item Panel");
        }

        /// <summary>
        /// 상점에 들어갔을 때 호출
        /// </summary>
        public void DisplayShopItem()
        {
            List<ItemData> buyPool = ReturnItemForSell(5);

            for (int i = 0; i < 5; i++)
            {
                if (i < buyPool.Count)
                {
                    _objList[i].Init(buyPool[i], _itemCombinator, _inventory);
                }
                else
                {
                    _objList[i].Init(null, null, null); // 아이템이 부족한 경우 품절 표시
                }
            }
        }

        /// <summary>
        /// quantity 개수 길이의 아이템 리스트 반환
        /// </summary>
        private List<ItemData> ReturnItemForLvlUp(int quantity)
        {
            List<ItemData> allItemPool = ItemDatabase.Instance.GetAllItems();
            List<ItemData> result = new();

            //CharacterClass currentClass = PlayerDataManager.Instance.Stats.CharacterClass;

            CharacterClass currentClass = CharacterClass.Sword;
            List<int> allowedUniqueIDs = new();
            switch (currentClass)
            {
                case CharacterClass.Sword:
                    allowedUniqueIDs.Add(10001);
                    break;
                case CharacterClass.Axe:
                    allowedUniqueIDs.Add(10002);
                    break;
                case CharacterClass.Magic:
                    allowedUniqueIDs.Add(10003);
                    break;
            }

            // 1. 인벤토리에 있는 Composite, Sub 아이템 개수
            int ownedCount = _inventory.GetAllItems()
                .Count(item => item.ItemRank == ItemRank.Sub || item.ItemRank == ItemRank.Composite);

            Debug.Log($"[ItemManager] ownedCount: {ownedCount}");

            int requiredOwnedMin = 0;
            if (ownedCount == 8) requiredOwnedMin = quantity;
            else if (ownedCount == 7) requiredOwnedMin = 2;
            else if (ownedCount == 6) requiredOwnedMin = 1;

            // 2. 우선 인벤토리에 있고, 아직 최대 레벨이 아닌 아이템 수집
            List<ItemData> fromInventory = _inventory.GetAllItems()
                .Where(item =>
                    (item.ItemRank == ItemRank.Sub || item.ItemRank == ItemRank.Composite) &&
                    item.ItemCurLevel < item.ItemMaxLevel &&
                    !item.IsComposited &&
                    (item.ItemType == ItemType.Attack || item.ItemType == ItemType.Passive))
                .ToList();

            fromInventory = fromInventory
                .OrderBy(_ => UnityEngine.Random.value)
                .Take(requiredOwnedMin)
                .ToList();

            result.AddRange(fromInventory);

            HashSet<int> fromInventoryIds = fromInventory.Select(i => i.ItemID).ToHashSet();

            // 3. 후보군에서 남은 슬롯 채우기
            List<ItemData> candidates = allItemPool
                .Where(item =>
                    (item.ItemType == ItemType.Attack || item.ItemType == ItemType.Passive) &&
                    item.ItemRank != ItemRank.Composite &&
                    !fromInventoryIds.Contains(item.ItemID) &&
                    !item.IsComposited &&
                    (item.ItemRank != ItemRank.Unique || allowedUniqueIDs.Contains(item.ItemID))
                )
                .OrderBy(_ => UnityEngine.Random.value)
                .Take(quantity - result.Count)
                .ToList();

            result.AddRange(candidates);

            return result;
        }


        private List<ItemData> ReturnItemForSell(int quantity)
        {
            List<ItemData> allItemPool = ItemDatabase.Instance.GetAllItems();
            List<ItemData> result = new();

            int ownedCount = _inventory.GetAllItems()
                .Count(item => item.ItemRank == ItemRank.Sub || item.ItemRank == ItemRank.Composite);

            int requiredOwnedMin = 0;
            if (ownedCount == 8) requiredOwnedMin = quantity;
            else if (ownedCount == 7) requiredOwnedMin = 4;
            else if (ownedCount == 6) requiredOwnedMin = 3;
            else if (ownedCount == 5) requiredOwnedMin = 2;
            else if (ownedCount == 4) requiredOwnedMin = 1;

            // 1. 인벤토리에 있고 최대 레벨 미만이며 Composite가 아닌 아이템만 수집
            List<ItemData> fromInventory = _inventory.GetAllItems()
                .Where(item =>
                    item.ItemRank == ItemRank.Sub && // Composite 제외
                    item.ItemCurLevel < item.ItemMaxLevel &&
                    !item.IsComposited &&
                    (item.ItemType == ItemType.Attack || item.ItemType == ItemType.Passive))
                .OrderBy(_ => UnityEngine.Random.value)
                .Take(requiredOwnedMin)
                .ToList();

            result.AddRange(fromInventory);

            HashSet<int> fromInventoryIds = fromInventory.Select(i => i.ItemID).ToHashSet();

            // 2. 전체 풀에서 후보 채우기 (Composite은 제외)
            List<ItemData> candidates = allItemPool
                .Where(item =>
                    item.ItemRank == ItemRank.Sub &&
                    !item.IsComposited &&
                    !fromInventoryIds.Contains(item.ItemID) &&
                    (item.ItemType == ItemType.Attack || item.ItemType == ItemType.Passive))
                .OrderBy(_ => UnityEngine.Random.value)
                .Take(quantity - result.Count)
                .ToList();

            result.AddRange(candidates);

            return result;
        }


        /// <summary>
        /// PlayerDataManager에게 인벤토리를 받기 (로드 및 씬 간 상태 공유를 위해)
        /// </summary>
        public void RecieveInventory()
        {
            _inventory.ClearInventory();

            foreach (var item in PlayerDataManager.Instance.InventoryItems)
            {
                _inventory.AddItem(item);
            }
        }

        /// <summary>
        /// PlayerDataManager에게 인벤토리를 보내기 (저장을 위해)
        /// </summary>
        public void SendInventory()
        {
            PlayerDataManager.Instance.InventoryItems.Clear();

            foreach (var item in _inventory.GetAllItems())
            {
                PlayerDataManager.Instance.InventoryItems.Add(item);
            }
        }

        public void Initialize()
        {
            _inventory ??= new ItemInventory();
            RecieveInventory();
        }

        public void Cleanup() { }


        public GameObject GetGameObject()
        {
            return gameObject;
        }

        private void TestInitInventory()
        {
            _inventory = new ItemInventory();
        }

        public void AssignGetItemButtons(List<GetItemButtonBehaviourClass> buttons)
        {
            _buttonList = buttons;
        }

        public void AssignBuyItemObjects(List<BuyItemObjBehaviourClass> objList)
        {
            _objList = objList;
        }

        private void TestInitUniqueItem()
        {
            // 플레이어의 현재 클래스에 따라 초기 유니크 아이템 부여
            //CharacterClass currentClass = PlayerDataManager.Instance.Stats.CharacterClass;

            CharacterClass currentClass = CharacterClass.Sword;

            int itemID = currentClass switch
            {
                CharacterClass.Sword => 10001,
                CharacterClass.Axe => 10002,
                CharacterClass.Magic => 10003,
                _ => 0
            };

            Debug.Log($"CharacterClass는 {currentClass}");

            if (itemID != 0)
            {
                ItemData data = ItemDatabase.Instance.GetItem(itemID);
                data.ItemCurLevel = 1;          // 초기 레벨 설정

                _inventory.AddItem(data);
            }

            TestLookInventory();

            if (_inventoryUI != null)
            {
                _inventoryUI.RenewUISlots();
            }
            else
            {
                Debug.Log($"[ItemManager] _inventoryUI가 null입니다");
            }
        }

        [ContextMenu("Test Look Inv")]
        private void TestLookInventory()
        {
            Debug.Log("=== [ItemManager] 현재 인벤토리 상태 출력 ===");

            List<ItemData> items = _inventory.GetAllItems();

            if (items.Count == 0)
            {
                Debug.Log("인벤토리에 아이템이 없습니다.");
                return;
            }

            foreach (var item in items)
            {
                Debug.Log($"- 이름: {item.ItemName}, ID: {item.ItemID}, 현재 레벨: {item.ItemCurLevel}, 최대 레벨: {item.ItemMaxLevel}, 조합됨: {item.IsComposited}");
            }

            Debug.Log("=== [ItemManager] 인벤토리 출력 끝 ===");
        }
    }
}
