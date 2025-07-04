using ProjectVS;
using ProjectVS.Data;
using ProjectVS.Manager;

using UnityEngine;

namespace ProjectVS.JDW
{
    public class Monster : MonoBehaviour
    {
        public float maxHp = 100;

        private float currentHp;

        [SerializeField] private int _exp = 1;
        [SerializeField] private int _gold = 5;

        private void Start()
        {
            currentHp = maxHp;
        }
        public void TakeDamage(float damage)
        {
            currentHp -= damage;
            Debug.Log($"몬스터 피격 : {damage}, 남은 체력 : {currentHp}");
            if (currentHp <= 0)
            {
                Die();
            }
        }



        private void Die()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                PlayerConfig player = playerObj.GetComponent<PlayerConfig>();
                if (player != null)
                {
                    player.ExpUp(_exp);
                    PlayerDataManager.Instance.Gold += _gold;
                    Debug.Log($"골드 흭득 : {_gold}, 현재 골드 : {PlayerDataManager.Instance.Gold}");
                    PlayerDataManager.Instance.TotalKills++;
                    Debug.Log($"총 처치수: {PlayerDataManager.Instance.TotalKills}");
                }
            }
            Debug.Log("몬스터 사망");

            Destroy(gameObject);
        }
    }
}
