using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterClass selectedClass;
    public PlayerStats stats;
    public Timer timer;

    public int gold = 100; // 기본 재화
    public bool isDead = false;

    private Animator anim;

    public List<string> inventory = new List<string>(); // 아이템 이름 저장용
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        stats = PlayerClassData.DefaultStats[selectedClass].Clone();
        Debug.Log($"선택 클래스: {selectedClass}, 체력: {stats.MaxHealth}, 공격력: {stats.Attack}, 방어력: {stats.Defense}, 공격속도 : {stats.AttackSpeed}, 이동속도 : {stats.MoveSpeed} 골드: {gold}");
        
    }
    public bool TryBuyItem(int price, int bonusHealth, int bonusAttack, int bonusDefense, float bonusAttackSpeed, float bonusMoveSpeed, string itemName)
    {
        if(gold < price)
        {
            Debug.Log("골드 부족");
            return false;
        }
        gold -= price;
        stats.Health = Mathf.Min(stats.Health + bonusHealth, stats.MaxHealth); // 회복이 최대체력을 넘기 못하게
        stats.Attack += bonusAttack;
        stats.Defense += bonusDefense;
        stats.AttackSpeed += bonusAttackSpeed;
        stats.MoveSpeed += bonusMoveSpeed;
        inventory.Add(itemName);

        Debug.Log($"{itemName} 구매 완료! 체력 +{bonusHealth}, 공격력 +{bonusAttack}, 방어력 +{bonusDefense},  공격속도 +{bonusAttackSpeed}, 이동속도 +{bonusMoveSpeed} 남은 골드: {gold}");
        return true;
    }
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        stats.Health -= damage;
        Debug.Log($"피해 : {damage}, 남은 체력 : {stats.Health}");

        if(stats.Health <= 0)
        {
            Die();
        }
    }
    private void Die()
    {
        isDead = true;

        if(anim !=null)
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
        bool LeveledUp = stats.AddExp(amount);
        Debug.Log($"경험치 흭득 : {amount}, 현재 경험치 : {stats.CurrentExp}/{stats.MaxExp}");
        if(LeveledUp)
        {
            Debug.Log($"레벨 업 {stats.Level}");
        }
    }
}
