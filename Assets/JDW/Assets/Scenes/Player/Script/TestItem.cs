using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 테스트용 아이템 스크립트
[System.Serializable]
public class TestItem
{
    public string name;
    public int bonusHealth;
    public int bonusAttack;
    public int bonusDefense;
    public float bonusAttackSpeed;
    public float bonusMoveSpeed;
    public TestItem(string name, int hp, int atk, int def = 0, float atkSpeed = 0f, float moveSpeed = 0 )
    {
        this.name = name;
        this.bonusHealth = hp;
        this.bonusAttack = atk;
        this.bonusDefense = def;
        this.bonusAttackSpeed = atkSpeed;
        this.bonusMoveSpeed = moveSpeed;
    }
}
