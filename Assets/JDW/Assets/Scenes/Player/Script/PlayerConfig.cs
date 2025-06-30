using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting.Antlr3.Runtime.Misc;

using UnityEngine;


namespace ProjectVS
{
    public class PlayerConfig : MonoBehaviour
    {
        public CharacterClass selectedClass;
        public PlayerStats Stats;
        public Timer timer;
        public Scanner scanner;

        public bool isDead = false;

        [SerializeField] private PixelUI.ValueBar _hpBar;

        private Animator anim;

        public List<string> inventory = new List<string>(); // 아이템 이름 저장용
        private void Awake()
        {
            anim = GetComponent<Animator>();
            scanner = GetComponent<Scanner>();
            Stats = PlayerClassData.DefaultStats[selectedClass].Clone();
        }
        private void Start()
        {
            Debug.Log($"선택 클래스: {selectedClass}, 체력: {Stats.CurrentMaxHp}, 공격력: {Stats.CurrentAtk}, 방어력: {Stats.CurrentDfs}, 공격속도 : {Stats.AtkSpd}, 이동속도 : {Stats.CurrentSpd} 골드: {Stats.Gold}");
            UpdateHpBar();
        }
        private void UpdateHpBar()
        {
            // 플레이어의 hpBar
            if (_hpBar != null && Stats != null)
            {
                _hpBar.MaxValue = Stats.CurrentMaxHp;
                _hpBar.CurrentValue = Stats.CurrentHp;
                _hpBar.SendMessage("UpdateUI", SendMessageOptions.DontRequireReceiver);
            }
        }
        public bool TryBuyItem(int price, int bonusHealth, int bonusAttack, int bonusDefense, float bonusAttackSpeed, float bonusMoveSpeed, string itemName)
        {
            if (Stats.Gold < price)
            {
                Debug.Log("골드 부족");
                return false;
            }
            Stats.Gold -= price;
            Stats.CurrentHp = Mathf.Min(Stats.CurrentHp + bonusHealth, Stats.CurrentMaxHp); // 회복이 최대체력을 넘기 못하게
            Stats.CurrentAtk += bonusAttack;
            Stats.CurrentDfs += bonusDefense;
            Stats.AtkSpd += bonusAttackSpeed;
            Stats.CurrentSpd += bonusMoveSpeed;
            inventory.Add(itemName);
            UpdateHpBar(); // 최대 체력이 오를 때 Hp바도 같이

            Debug.Log($"{itemName} 구매 완료! 체력 +{bonusHealth}, 공격력 +{bonusAttack}, 방어력 +{bonusDefense},  공격속도 +{bonusAttackSpeed}, 이동속도 +{bonusMoveSpeed} 남은 골드: {Stats.Gold}");
            return true;
        }
        public void TakeDamage(float damage)
        {
            if (isDead) return;

            Stats.CurrentHp -= damage;
            Debug.Log($"피해 : {damage}, 남은 체력 : {Stats.CurrentHp}");
            UpdateHpBar();  // Hp바 연동

            if (Stats.CurrentHp <= 0)
            {
                Die();
            }
        }
        private void Die()
        {
            isDead = true;

            if (anim != null)
                anim.SetTrigger("IsDead"); // 사망 애니메이션

            GetComponent<PlayerFlipbyMouse>().enabled = false;
            GetComponent<PlayerMove>().enabled = false;
            timer.PauseTimer(); // 플레어 사망시 시간 멈춤
            AttackPosition attack = GetComponentInChildren<AttackPosition>();
            if (attack != null)
                attack.enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 사망시 플레이어 스탑

            // TODO : 플레이어 사망시 UI출력 또는 씬 이동 로직 추가

            Debug.Log("플레이어 사망");
        }
        public void ExpUp(float amount)
        {
            if (isDead) return;
            bool LeveledUp = Stats.AddExp(amount);
            Debug.Log($"경험치 흭득 : {amount}, 현재 경험치 : {Stats.CurrentExp}/{Stats.MaxExp}");
            if (LeveledUp)
            {
                Debug.Log($"레벨 업 {Stats.Level}");
                UpdateHpBar();  // 레벨업 시 체력바 갱신
            }
        }
        void OnDestroy()
        {
            if (GameManager.Instance != null)
                GameManager.Instance.SavePlayerStats(Stats);
        }
       
    }
}
