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
            if (other.CompareTag("Player")) // 플레이어 태그가 붙어있는 대상과 충돌
            {
                PlayerConfig player = other.GetComponent<PlayerConfig>();
                TestPlayerInventory inventory = other.GetComponent<TestPlayerInventory>();
                if(player != null && ItemData != null)
                {
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
                        Destroy(gameObject); // 아이템 제거
                    }
                }                     
            }
        }
    }
}
