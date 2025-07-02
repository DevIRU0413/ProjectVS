using ProjectVS;

using UnityEngine;

namespace ProjectVS.JDW
{
    public class StoreItem : MonoBehaviour
    {
        public string itemName = "임시 아이템";
        public int price;
        public int recovery;
        public int bonusAttack;
        public int bonusDefense;
        public float bonusAttackSpeed;
        public float bonusMoveSpeed;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) // 플레이어 태그가 붙어있는 대상과 충돌
            {
                PlayerConfig player = other.GetComponent<PlayerConfig>();
                if (player != null)
                {
                    bool bought = player.TryBuyItem(price, recovery, bonusAttack, bonusDefense, bonusAttackSpeed, bonusMoveSpeed, itemName);
                    if (bought)
                    {
                        Destroy(gameObject); // 아이템 제거
                    }
                }
            }
        }
    }
}
