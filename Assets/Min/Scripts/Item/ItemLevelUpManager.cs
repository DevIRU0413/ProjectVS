using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;



namespace ProjectVS.Item.ItemLevelUpManager
{
    public class ItemLevelUpManager : MonoBehaviour
    {
        private List<ItemData> _itemPool = new();
        private List<ItemData> _allItemPool = new();


        private ItemInventory _inventory; // 이거 누가 넘겨줘야되지?
                                  // 본 클래스가 싱글톤으로써 인스턴스를 계속 갖고 있어줘야되나?
                                  // 아니면 씬 넘어갈 때, PlayerData에 넘기고 다시 해당 씬에서 주고 받기?

        private ItemCombinator _itemCombinator;

        private void Awake()
        {
            _inventory = new(); // 아니면 외부에서 받아야 될 듯
            _itemCombinator = new(ItemDatabase.Instance.GetAllItems());
        }


        /// <summary>
        /// 레벨 업 시 호출
        /// </summary>
        public List<ItemData> ReturnItemOnLevelUp()
        {
            return ReturnItem(3);
        }

        /// <summary>
        /// 상점에 들어갔을 때 호출
        /// </summary>
        public List<ItemData> ReturnItemOnShop()
        {
            return ReturnItem(5);
        }

        /// <summary>
        /// quantity 개수 길이의 아이템 리스트 반환
        /// </summary>
        private List<ItemData> ReturnItem(int quantity)
        {
            List<ItemData> allItemPool = ItemDatabase.Instance.GetAllItems();
            List<ItemData> inventory = _inventory.GetAllItems();

            List<ItemData> candidates;

            // 인벤토리가 다 찼으면
            if (inventory.Count == 8)
            {
                candidates = inventory.Where(item =>
                (item.ItemType == ItemType.Attack ||     // 액티브 아이템인지
                item.ItemType == ItemType.Passive) //&&    // 패시브 아이템인지
                //!_inventory.최대레벨검사(item.ItemID)    // max레벨인지 검사
                ).ToList();
            }
            // 인벤토리가 비어있으면 
            else
            {
                // 전체 아이템에서 필터링
                candidates = allItemPool.Where(item =>
                (item.ItemType == ItemType.Attack ||     // 액티브 아이템인지
                item.ItemType == ItemType.Passive) &&    // 패시브 아이템인지
                //!_inventory.조합되어 사라진 아이템인지 &&

                (_inventory.HasItem(item.ItemID) //|| // !item.최대레벨검사)  // 인벤토리에 있으면서 최대 레벨이 아닌지
                )
                &&
                (item.ItemRank != ItemRank.Composite) // || _inventory.해금된건지 검사)
                )
                .ToList();
            }

            // 랜덤 추출
            _itemPool = candidates.OrderBy(x => Random.value).Take(quantity).ToList();

            return _itemPool;
        }
    }
}
