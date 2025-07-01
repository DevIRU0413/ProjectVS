using System.Collections;
using System.Collections.Generic;

using ProjectVS.Data;
using ProjectVS.Manager;
using ProjectVS.Unit;

using Unity.VisualScripting.Antlr3.Runtime.Misc;

using UnityEngine;

using CharacterSelectionDataClass = ProjectVS.CharacterSelectionData.CharacterSelectionData.CharacterSelectionData;



namespace ProjectVS
{
    public class PlayerConfig : MonoBehaviour
    {
        public CharacterClass selectedClass;
        public PlayerStats Stats;
        public Timer timer;
        public Scanner scanner;
        public PlayerDataManager playerDataManager;
        public AttackPosition attackPosition;

        public bool isDead = false;

        [SerializeField] private PixelUI.ValueBar _hpBar;

        private Animator anim;
        public List<string> inventory = new List<string>(); // 아이템 이름 저장용
        private void Awake()
        {
            anim = GetComponent<Animator>();
            scanner = GetComponent<Scanner>();
          //  Stats = PlayerStats.TestStats(selectedClass); // playerStats에서 클래스 데이터를 사용 할 경우 이걸사용
        }
        private void Start()
        {

            UpdateHpBar(); // 초기 체력바 표시
        }

        public void ApplyStatsFromData(CharacterSelectionDataClass data)
        {
            // TSV 데이터 기반으로 Stats 초기화
            Stats = new PlayerStats(1, selectedClass, data.HP, data.Attack, data.Defense, data.MoveSpeed, data.AttackSpeed);
            Stats.CurrentHp = data.HP;
            UpdateHpBar();
            Debug.Log($"[TSV 적용됨] ATK: {Stats.BaseAtk}, DEF: {Stats.BaseDfs}, HP: {Stats.CurrentHp}, SPD: {Stats.BaseSpd}");
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
        public bool TryBuyItem(int price, int bonusHp, int bonusAtk, int bonusDfs, float bonusAtkSpd, float bonusSpd, string itemName)
        {
            if (playerDataManager.gold < price)
            {
                Debug.Log("골드 부족");
                return false;
            }
            playerDataManager.gold -= price;  
   
            Stats.CurrentHp = Mathf.Min(Stats.CurrentHp + bonusHp, Stats.CurrentMaxHp);
            inventory.Add(itemName);
            UpdateHpBar(); // 최대 체력이 오를 때 Hp바도 같이

            Debug.Log($"{itemName} 구매 완료! 체력 +{bonusHp}, 공격력 +{bonusAtk}, 방어력 +{bonusDfs},  공격속도 +{bonusAtkSpd}, 이동속도 +{bonusSpd} 남은 골드: {playerDataManager.gold}");
            return true;
        }
        public void TakeDamage(float damage)
        {
            if (isDead) return;

            Stats.CurrentHp -= damage;
            Debug.Log($"피해 : {damage}, 남은 체력 : {Stats.CurrentHp}");
            UpdateHpBar();  // Hp바 연동

            if (Stats.CurrentHp <= 0)
                  Die();
         
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
        public void ApplyStatsFromData(CharacterSelectionDataClass data, int classIndex)
        {
            Stats = new PlayerStats(1, selectedClass, data.HP, data.Attack, data.Defense, data.MoveSpeed, data.AttackSpeed);
            Stats.CurrentHp = data.HP;
            UpdateHpBar();

            // 공격 위치 설정
            attackPosition = GetComponentInChildren<AttackPosition>();
            if (attackPosition != null)
            {
                switch (classIndex)
                {
                    case 0: attackPosition.SwitchCoroutine(attackPosition.Axe()); break;
                    case 1: attackPosition.SwitchCoroutine(attackPosition.Sword()); break;
                    case 2: attackPosition.SwitchCoroutine(attackPosition.Fire()); break;
                }
            }
        }

    }
}
