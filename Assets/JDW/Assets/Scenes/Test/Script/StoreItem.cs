using ProjectVS;
using ProjectVS.Data;

using UnityEngine;

namespace ProjectVS.JDW
{
    public class StoreItem : MonoBehaviour
    {
        public TestItemData ItemData;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) // 플레이어 태그 확인
            {
                PlayerConfig player = other.GetComponent<PlayerConfig>();
                TestPlayerInventory inventory = other.GetComponent<TestPlayerInventory>();

                if (player != null && inventory != null && ItemData != null)
                {
                    //  아이템 개수 제한 먼저 체크
                    if (inventory.items.Count >= inventory.MaxItemCount)
                    {
                        Debug.Log("인벤토리가 가득 찼습니다. 아이템을 획득할 수 없습니다.");
                        return;
                    }

                    // 이후 정상 처리
                    bool bought = player.TryBuyItem(
                        ItemData.Price,
                        ItemData.BonusHP,
                        ItemData.BonusAtk,
                        ItemData.BonusDfs,
                        ItemData.BonusAtkSpd,
                        ItemData.BonusSpd,
                        ItemData.Name);

                    if (bought)
                    {
                        inventory.AddItem(ItemData);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}
