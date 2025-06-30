using System.Collections.Generic;

using ProjectVS;
using ProjectVS.Unit;

using UnityEngine;

[System.Serializable]
public class PlayerStats : UnitStats
{
    public int Level = 1;                   // 현재 레벨
    public CharacterClass CharacterClass;   // 직업

    public float CurrentExp = 0f;           // 현재 경험치
    public float MaxExp = 100f;             // 레벨업 까지 필요한 경험치
    public int Gold = 100;                  // 플레이어 재화


    public void GainReward(float exp, int gold)
    {
        Gold += gold;
        AddExp(exp);
    }
    public bool AddExp(float amount)
    {
        CurrentExp += amount;
        if (CurrentExp >= MaxExp)
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
                CurrentMaxHp += 10;
                CurrentAtk += 8;
                CurrentDfs += 2;
                CurrentSpd += 0;
                AtkSpd += 0;
                break;
            case CharacterClass.Sword:
                CurrentMaxHp += 7;
                CurrentAtk += 5;
                CurrentDfs += 8;
                CurrentSpd += 0;
                AtkSpd += 0;
                break;
            case CharacterClass.Magic:
                CurrentMaxHp += 5;
                CurrentAtk += 7;
                CurrentDfs += 8;
                CurrentSpd += 0;
                AtkSpd += 0;
                break;
        }
        Debug.Log($"레벨 업, 현재 레벨 : {Level}, 남은 경험치 : {CurrentExp}, 다음 레벨까지 : {MaxExp}");
    }
    public PlayerStats Clone()
    {
        //원본 값이 수정되지 않도록 클론을 이용
        PlayerStats clone = new PlayerStats(1, CharacterClass, CurrentMaxHp, CurrentAtk, CurrentDfs, CurrentSpd, AtkSpd);
        clone.Level = this.Level;
        clone.CurrentExp = this.CurrentExp;
        clone.MaxExp = this.MaxExp;
        clone.Gold = this.Gold;
        return clone;
    }

    //플레이어를 생성할 때 사용할 수 있도록 값을 받음
    public PlayerStats(int level, CharacterClass charClass, float baseMaxHp, float baseAtk, float baseDfs, float baseSpd, float baseAtkSpd)
        : base(baseMaxHp,  baseAtk,  baseDfs,  baseSpd,  baseAtkSpd)
    {
        Level = level;
        CharacterClass = charClass;
    }
}
