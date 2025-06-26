using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEditor.UIElements;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterClass selectedClass;
    public PlayerStats stats;
    public int gold = 100; // 재화

    public List<string> inventory = new List<string>(); // 아이템 이름 저장용

    private void Start()
    {
        stats = PlayerClassData.DefaultStats[selectedClass].Clone();
        Debug.Log($"선택 클래스: {selectedClass}, 체력: {stats.Health}, 공격력: {stats.Attack}, 방어력: {stats.Defense}, 공격속도 : {stats.AttackSpeed}, 이동속도 : {stats.MoveSpeed} 골드: {gold}");
    }
    public bool TryBuyItem(int price, int bonusHealth, int bonusAttack, int bonusDefense, float bonusAttackSpeed, float bonusMoveSpeed, string itemName)
    {
        if(gold < price)
        {
            Debug.Log("골드 부족");
            return false;
        }
        gold -= price;
        stats.Health += bonusHealth;
        stats.Attack += bonusAttack;
        stats.Defense += bonusDefense;
        stats.AttackSpeed += bonusAttackSpeed;
        stats.MoveSpeed += bonusMoveSpeed;
        inventory.Add(itemName);

        Debug.Log($"{itemName} 구매 완료! 체력 +{bonusHealth}, 공격력 +{bonusAttack}, 방어력 +{bonusDefense},  공격속도 +{bonusAttackSpeed}, 이동속도 +{bonusMoveSpeed} 남은 골드: {gold}");
        return true;
    }
}
