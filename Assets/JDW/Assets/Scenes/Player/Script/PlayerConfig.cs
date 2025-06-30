using System.Collections.Generic;

using ProjectVS.Interface;
using ProjectVS.Manager;

using UnityEngine;

namespace ProjectVS
{
    public class PlayerConfig : MonoBehaviour, IDamageable
    {
        public PlayerStats Stats { get; private set; }
        private PlayerDataManager _playerData;

        public Scanner scanner;

        private Animator _anim;
        private PlayerFlipbyMouse _playerFlipbyMouse;
        private PlayerMove _playerMove;

        public bool isDead = false;

        public List<string> inventory = new List<string>(); // 아이템 이름 저장용

        private void Awake()
        {
            Stats = PlayerDataManager.ForceInstance.stats;
            scanner = GetComponent<Scanner>();

            _anim = GetComponent<Animator>();
            _playerFlipbyMouse = GetComponent<PlayerFlipbyMouse>();
            _playerMove = GetComponent<PlayerMove>();
        }

        private void Start()
        {
            _playerData = PlayerDataManager.ForceInstance;
            Stats = _playerData.stats;

            Debug.Log($"선택 클래스: {Stats.CharacterClass}, 체력: {Stats.CurrentMaxHp}, 공격력: {Stats.CurrentAtk}, 방어력: {Stats.CurrentDfs}, 공격속도: {Stats.AtkSpd}, 이동속도: {Stats.CurrentSpd}, 골드: {_playerData.gold}");
        }

        // 아이템쪽에 있어야되는 기능(여기에서는 아이템을 샀다. 라는 명시적 코드만 필요함.)
        public bool TryBuyItem(int price, int bonusHealth, int bonusAttack, int bonusDefense, float bonusAttackSpeed, float bonusMoveSpeed, string itemName)
        {
            var stats = _playerData.stats;

            if (_playerData.gold < price)
            {
                Debug.Log("골드 부족");
                return false;
            }

            _playerData.gold -= price;

            stats.SetIncreaseBaseStats(UnitStaus.MaxHp, stats.CurrentMaxHp + bonusHealth);

            stats.SetIncreaseBaseStats(UnitStaus.Atk, bonusAttack);
            stats.SetIncreaseBaseStats(UnitStaus.Dfs, bonusDefense);
            stats.SetIncreaseBaseStats(UnitStaus.AtkSpd, bonusAttackSpeed);
            stats.SetIncreaseBaseStats(UnitStaus.Spd, bonusMoveSpeed);

            inventory.Add(itemName);

            Debug.Log($"{itemName} 구매 완료! 체력 +{bonusHealth}, 공격력 +{bonusAttack}, 방어력 +{bonusDefense}, 공격속도 +{bonusAttackSpeed}, 이동속도 +{bonusMoveSpeed} 남은 골드: {_playerData.gold}");
            return true;
        }

        public void TakeDamage(DamageInfo info)
        {
            if (isDead) return;

            var stats = _playerData.stats;

            stats.CurrentHp -= info.Amount;
            Debug.Log($"피해 : {info}, 남은 체력 : {stats.CurrentHp}");

            if (stats.CurrentHp <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            isDead = true;

            if (_anim != null)
                _anim.SetTrigger("IsDead"); // 사망 애니메이션

            _playerFlipbyMouse.enabled = false;
            _playerMove.enabled = false;

            AttackPosition attack = GetComponentInChildren<AttackPosition>();
            if (attack != null)
                attack.enabled = false;

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;

            // TODO : 사망 시 UI 출력 또는 씬 이동 로직 추가
            Debug.Log("플레이어 사망");
        }

        public void ExpUp(float amount)
        {
            if (isDead) return;

            var stats = _playerData.stats;

            bool leveledUp = stats.AddExp(amount);
            Debug.Log($"경험치 획득 : {amount}, 현재 경험치 : {stats.CurrentExp}/{stats.MaxExp}");

            if (leveledUp)
            {
                Debug.Log($"레벨 업! 현재 레벨: {stats.Level}");
            }
        }
    }
}
