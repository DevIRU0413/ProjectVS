using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using ProjectVS.Manager;
using GetItemButtonBehaviourClass = ProjectVS.Item.GetItemButtonBehaviour.GetItemButtonBehaviour;
using BuyItemObjBehaviourClass = ProjectVS.Item.BuyItemObjBehaviour.BuyItemObjBehaviour;
using ProjectVS.Utils.UIManager;
using ProjectVS.Util;
using ProjectVS.Interface;
using System;
using UnityEngine.InputSystem;



namespace ProjectVS.Item.ItemManager
{
    /// <summary>
    /// 인벤토리 조작 및 인스턴스 유지 접근, 아이템 조합기 보유, UI 연결 역할 수행
    /// </summary>

    // TODO: 역할 분리 가능할 듯
    public class ItemManager : SimpleSingleton<ItemManager>, IManager
    {
        private List<ItemData> _itemPool = new();

        private ItemCombinator _itemCombinator;
        private ItemInventory _inventory;
        public ItemInventory Inventory => _inventory;

        [Header("아이템 획득 및 구매 연동")]
        [SerializeField] private List<GetItemButtonBehaviourClass> _buttonList = new();
        [SerializeField] private List<BuyItemObjBehaviourClass> _objList = new();

        public int Priority => (int)ManagerPriority.ItemManager;
        public bool IsDontDestroy => IsDontDestroyOnLoad;

        public event Action OnInventoryChanged;

        protected override void Awake()
        {
            TestInitInventory(); // 테스트용 인벤토리 초기화, 추후 삭제해야 됨

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


        /// <summary>
        /// 레벨 업 시 호출
        /// </summary>
        [ContextMenu("Test Level Up")]
        public void LevelUpItem()
        {
            List<ItemData> levelUpPool = ReturnItem(3);

            for (int i = 0; i < 3; i++)
            {
                _buttonList[i].Init(levelUpPool[i], _itemCombinator, _inventory);
            }

            UIManager.Instance.Show("Level Up Item Panel");
        }

        /// <summary>
        /// 상점에 들어갔을 때 호출
        /// </summary>
        public void DisplayShopItem()
        {
            List<ItemData> buyPool = ReturnItem(5);

            for (int i = 0; i < 3; i++)
            {
                _objList[i].Init(buyPool[i], _itemCombinator, _inventory);
            }
        }

        /// <summary>
        /// quantity 개수 길이의 아이템 리스트 반환
        /// </summary>
        private List<ItemData> ReturnItem(int quantity)
        {
            //List<ItemData> allItemPool = ItemDatabase.Instance.GetAllItems();
            //List<ItemData> inventory = _inventory.GetAllItems();

            //List<ItemData> candidates;

            //// 인벤토리가 다 찼으면
            //if (inventory.Count == 8)
            //{
            //    candidates = inventory.Where(item =>
            //        (item.ItemType == ItemType.Attack ||     // 액티브 아이템인지
            //         item.ItemType == ItemType.Passive) &&   // 패시브 아이템인지
            //        !_inventory.GetItemsByID(item.ItemID).All(i => i.ItemCurLevel >= i.ItemMaxLevel) && // 최대 레벨이 아닌지
            //        !item.IsComposited                       // 조합되어 나오면 안되는 아이템이 아닌지
            //    ).ToList();
            //}
            //// 인벤토리가 비어있으면 
            //else
            //{
            //    candidates = allItemPool.Where(item =>
            //        (item.ItemType == ItemType.Attack ||     // 액티브 아이템인지
            //         item.ItemType == ItemType.Passive) &&   // 패시브 아이템인지
            //        (
            //            _inventory.HasItem(item.ItemID) ||   // 인벤토리에 있는 아이템이거나
            //            !_inventory.GetItemsByID(item.ItemID).All(i => i.ItemCurLevel >= i.ItemMaxLevel) // 최대 레벨이 아닌 아이템
            //        ) &&
            //        (!item.IsComposited) &&                  // 조합되어 나오면 안되는 아이템이 아닌지
            //        (item.ItemRank != ItemRank.Composite ||  // 조합 아이템이 아닌 경우 포함
            //         _inventory.HasItem(item.ItemID))        // 조합 아이템이라면 해금(획득) 여부 확인
            //    ).ToList();
            //}

            //// 랜덤 추출
            //_itemPool = candidates.OrderBy(x => UnityEngine.Random.value).Take(quantity).ToList();

            //return _itemPool;

            List<ItemData> allItemPool = ItemDatabase.Instance.GetAllItems();

            List<ItemData> candidates = new();

            foreach (var item in allItemPool)
            {
                // 공격형 또는 패시브 아이템만 대상으로 함
                if (item.ItemType != ItemType.Attack && item.ItemType != ItemType.Passive)
                    continue;

                // 조합되어 나오면 안 되는 아이템은 제외
                if (item.IsComposited)
                    continue;

                // 조합 아이템이면 해금 여부 확인
                if (item.ItemRank == ItemRank.Composite && !_inventory.HasItem(item.ItemID))
                    continue;

                // 인벤토리에 동일 ID 아이템이 있는 경우, 그 중 아직 최대 레벨이 아닌 게 있는지 확인
                if (_inventory.HasItem(item.ItemID))
                {
                    bool hasNonMaxItem = _inventory.GetItemsByID(item.ItemID)
                        .Any(i => i.ItemCurLevel < i.ItemMaxLevel && !i.IsComposited);

                    if (!hasNonMaxItem)
                        continue; // 전부 만렙이거나 조합됨 → 스킵
                }

                candidates.Add(item);
            }

            // 무작위 선택
            _itemPool = candidates.OrderBy(x => UnityEngine.Random.value).Take(quantity).ToList();

            return _itemPool;
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

            List<ItemData> items = ItemDatabase.Instance.GetAllItems();

            foreach (var item in items)
            {
                if (item.ItemRank == ItemRank.Composite) continue;
                _inventory.AddItem(item);
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
