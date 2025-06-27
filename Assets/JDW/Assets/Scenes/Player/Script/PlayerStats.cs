using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterClass
{// 플레이어가 선택 가능한 클레스
    Sword, // 검
    Axe, // 도끼
    Magic // 마법
}
    [System.Serializable]
    public class PlayerStats
    {
    public int Level = 1; // 현재 레벨
    public float CurrentExp = 0f; // 현재 경험치
    public float MaxExp = 100f; // 레벨업 까지 필요한 경험치
    public int Gold = 100; // 플레이어 재화

    public float MaxHealth;       //최대 체력
    public float Health;      //현재 체력
    public float Attack;      //공격력
    public float Defense;     //방어력
    public float MoveSpeed;   //이동속도
    public float AttackSpeed; //공격속도

    public CharacterClass CharacterClass;

    public void GainReward(float exp, int gold)
    {
        Gold += gold;
        AddExp(exp);
    }
    public bool AddExp(float amount)
    {
        CurrentExp += amount;
        if(CurrentExp >= MaxExp)
        {
            LevelUp();
            return true;
        }
        return false;
    }
    private void LevelUp()
    {
        CurrentExp -= MaxExp;
        Level++;
        MaxExp *= 1.2f; // 다음 레벨업까지 필요한 경험치 증가

        switch (CharacterClass)
        {
            case CharacterClass.Axe:
        MaxHealth += 10;
        Attack += 8;
        Defense += 2;
        MoveSpeed += 0;
        AttackSpeed += 0;
                break;
            case CharacterClass.Sword:
                MaxHealth += 7;
                Attack += 5;
                Defense += 8;
                MoveSpeed += 0;
                AttackSpeed += 0;
                break;
            case CharacterClass.Magic:
                MaxHealth += 5;
                Attack += 7;
                Defense += 8;
                MoveSpeed += 0;
                AttackSpeed += 0;
                break;
        }
        Debug.Log($"레벨 업, 현재 레벨 : {Level}, 남은 경험치 : {CurrentExp}, 다음 레벨까지 : {MaxExp}");
    }
        //플레이어를 생성할 때 사용할 수 있도록 값을 받음
        public PlayerStats(CharacterClass charClass, float health, float attack, float defense, float moveSpeed, float attackSpeed)
        {
        CharacterClass = charClass;
        MaxHealth = health;
        Health = health;
            Attack = attack;
            Defense = defense;
            MoveSpeed = moveSpeed;
            AttackSpeed = attackSpeed;
        }
        public PlayerStats Clone()
        {//원본 값이 수정되지 않도록 클론을 이용
        PlayerStats clone = new PlayerStats(CharacterClass, MaxHealth, Attack, Defense, MoveSpeed, AttackSpeed);
        clone.Level = this.Level;
        clone.CurrentExp = this.CurrentExp;
        clone.MaxExp = this.MaxExp;
        clone.Gold = this.Gold;
        return clone;
    }
    }
    public static class PlayerClassData
    {// 클레스별 초기 스탯을 설정해 놓은 보관함
        public static readonly Dictionary<CharacterClass, PlayerStats> DefaultStats = new Dictionary<CharacterClass, PlayerStats>
        {
            {CharacterClass.Axe, new PlayerStats(CharacterClass.Axe, 20,30,10,3,0.5f) }, // 도끼 클레스는 튼튼하고 강하지만 느림
            {CharacterClass.Sword, new PlayerStats(CharacterClass.Sword, 25,20,8,6,2) },// 검 클레스는 안정적이고 빠름
            {CharacterClass.Magic, new PlayerStats(CharacterClass.Magic, 15,25,5,4,1) } // 마법 클레스는 공격 사거리가 길지만 약함
        };
    }

