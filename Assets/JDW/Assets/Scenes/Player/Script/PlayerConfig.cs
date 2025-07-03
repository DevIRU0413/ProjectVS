using System.Collections;
using System.Collections.Generic;

using ProjectVS.Data;
using ProjectVS.Manager;
using ProjectVS.Unit;
using ProjectVS.Unit.Player;
using ProjectVS.Utils.UIManager;

using Unity.VisualScripting.Antlr3.Runtime.Misc;

using UnityEngine;

using CharacterSelectionDataClass = ProjectVS.CharacterSelectionData.CharacterSelectionData.CharacterSelectionData;



namespace ProjectVS.JDW
{
    public class PlayerConfig : MonoBehaviour
    {
        [HideInInspector]public PlayerDataManager PlayerDataManager;

        public CharacterClass SelectedClass;
        public Timer Timer;
        public Scanner Scanner;
        public AttackPosition AttackPosition;

        public bool IsDead = false;
        private BattleSceneUI _uiManager;
        private bool _statsApplied = false;
        [SerializeField] public PlayerStats Stats;
        [SerializeField] private PixelUI.ValueBar _hpBar;

        private Animator _anim;
        public List<string> Inventory = new List<string>(); // 아이템 이름 저장용
        private void Awake()
        {
            _anim = GetComponent<Animator>();
            Scanner = GetComponent<Scanner>();
            //  Stats = PlayerStats.TestStats(SelectedClass); // playerStats에서 클래스 데이터를 사용 할 경우 이걸사용
            // 타이머 자동 할당
            if (Timer == null)
            {
                Timer = FindObjectOfType<Timer>();
                if (Timer == null)
                {
                    Debug.LogWarning("Timer를 씬에서 찾을 수 없습니다.");
                }
            }
            Stats = PlayerDataManager.Instance.Stats;
        }
        private void Start()
        {
            _uiManager = FindObjectOfType<BattleSceneUI>();
            UpdateHpBar(); // 초기 체력바 표시
        }

        public void ApplyStatsFromData(CharacterSelectionDataClass data, int classIndex)
        {
            if (_statsApplied) return; // TSV 데이터로 이미 적용했으면 그 다음부터는 무시
            // TSV 데이터 기반으로 Stats 초기화
           // Stats = new PlayerStats(1, SelectedClass, data.HP, data.Attack, data.Defense, data.MoveSpeed, data.AttackSpeed);
          //  Stats.CurrentHp = data.HP;
            var stats = PlayerDataManager.Instance.Stats;

            stats.Level = 1;                                           //////////
            stats.CharacterClass = SelectedClass;                      //
                                                                       //
            stats.SetBaseStat(UnitStaus.MaxHp, data.HP);               //
            stats.SetBaseStat(UnitStaus.Atk, data.Attack);             //
            stats.SetBaseStat(UnitStaus.Dfs, data.Defense);            //     데이터 매니저와 연결
            stats.SetBaseStat(UnitStaus.Spd, data.MoveSpeed);          //
            stats.SetBaseStat(UnitStaus.AtkSpd, data.AttackSpeed);     //
                                                                       //
            stats.CurrentHp = data.HP;                                 //
            stats.CurrentExp = 0;                                      //
            stats.MaxExp = 100;                                        //////////////

            Stats = stats;

            UpdateHpBar();
            _statsApplied = true;

            // 공격 위치 설정
         //  AttackPosition = GetComponentInChildren<AttackPosition>();
         //  if (AttackPosition != null)
         //  {
         //     // switch (classIndex)
         //     // {
         //     //     case 0: AttackPosition.SwitchCoroutine(AttackPosition.Axe()); break;
         //     //     case 1: AttackPosition.SwitchCoroutine(AttackPosition.Sword()); break;
         //     //     case 2: AttackPosition.SwitchCoroutine(AttackPosition.Fire()); break;
         //     // }
         //  }
            Debug.Log($"[TSV 적용됨] ATK: {Stats.CurrentAtk}, DEF: {Stats.CurrentDfs}, HP: {Stats.CurrentHp}, SPD: {Stats.CurrentSpd}");
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
            float hpRatio = Stats.CurrentHp / Stats.CurrentMaxHp;
            _uiManager?.UpdatePortrait(hpRatio);
        }
        public bool TryBuyItem(int price, int bonusHp, int bonusAtk, int bonusDfs, float bonusAtkSpd, float bonusSpd, string itemName)
        {
            if (PlayerDataManager.Instance.Gold < price)
            {
                Debug.Log("골드 부족");
                return false;
            }
            PlayerDataManager.Instance.Gold -= price;

            // 기존 시스템을 활용해서 스탯 증가
            Stats.AddBaseStats(bonusHp, bonusAtk, bonusDfs, bonusSpd, bonusAtkSpd);

            // 체력 회복은 별도로 처리
            Stats.CurrentHp = Mathf.Min(Stats.CurrentHp + bonusHp, Stats.CurrentMaxHp);
            Inventory.Add(itemName);
            UpdateHpBar(); // 최대 체력이 오를 때 Hp바도 같이

            Debug.Log($"{itemName} 구매 완료! 체력 +{bonusHp}, 공격력 +{bonusAtk}, 방어력 +{bonusDfs},  공격속도 +{bonusAtkSpd}, 이동속도 +{bonusSpd} 남은 골드: {PlayerDataManager.Instance.Gold}");
            return true;
        }
        public void TakeDamage(float damage)
        {
            if (IsDead) return;
            FindObjectOfType<CameraFollow>()?.ShakeCamera(0.3f, 0.5f);
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
            IsDead = true;

            if (_anim != null)
            {
                _anim.SetTrigger("IsDead"); // 사망 애니메이션
            }

            GetComponent<PlayerFlipbyMouse>().enabled = false;
            GetComponent<PlayerMove>().enabled = false;
            Timer.PauseTimer(); // 플레어 사망시 시간 멈춤
            Timer.SendMessage("Die"); // 사망시 텍스트에 DIe 표기
            _uiManager.ShowDeathResult();

            AttackPosition attack = GetComponentInChildren<AttackPosition>(); // 사망시 플레이어 공격 멈춤
            if (attack != null)
                attack.enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector2.zero; // 사망시 플레이어 스탑

            if (Timer != null)
            {
                int battleCount = PlayerDataManager.Instance.BattleSceneCount; // 현재 카운트된 전투 씬
                float survivedTime = 900f - Timer.CurrentTime; // 이번 전투씬 생존 시간
                float totalPlayTime = (battleCount - 1) * 900f + survivedTime; // 씬 포함 모든 전투 플레이 시간

                PlayerDataManager.Instance.TotalPlayTime = totalPlayTime;

                Debug.Log($"[플레이타임 계산] 전투씬 수: {battleCount}, 생존시간: {survivedTime}초, 총 플레이타임: {totalPlayTime}초");
            }
            Debug.Log("플레이어 사망");
        }
        public void ExpUp(float amount)
        {
            if (IsDead) return;

            bool leveledUp = Stats.AddExp(amount);
            Debug.Log($"경험치 흭득 : {amount}, 현재 경험치 : {Stats.CurrentExp}/{Stats.MaxExp}");
            if (leveledUp)
            {
                Debug.Log($"레벨 업 {Stats.Level}");
                UpdateHpBar();  // 레벨업 시 체력바 갱신
            }
        }   
    }
}
