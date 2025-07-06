using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TestItemData
{
    public string Name;
    public Sprite Icon;
    public int BonusHP;
    public int BonusAtk;
    public int BonusDfs;
    public float BonusAtkSpd;
    public float BonusSpd;
    public int Price;
    public TestItemData(string name, Sprite icon, int hp, int atk, int dfs, float atkSpd, float spd, int price)
    {
        this.Name = name;
        this.Icon = icon;
        this.BonusHP = hp;
        this.BonusAtk = atk;
        this.BonusDfs = dfs;
        this.BonusAtkSpd = atkSpd;
        this.BonusSpd = spd;
        this.Price = price;
    }
}
