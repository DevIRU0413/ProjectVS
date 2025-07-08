using System;

using ProjectVS.Unit;
using UnityEngine;

namespace ProjectVS.Unit.Monster
{
    [Serializable]
    public class MonsterStatsConfig : UnitStatsConfig
    {
        public string MonsterName; // 이름
        public float SizeValue;    // 사이즈

        public MonsterClass Class; // 클래스

        public int Exp;            // 경험치

        public int DropGold;       // 골드
        public int DropGoldPer;    // 골드 확률

        public int DropDiamond;    // 다이야
        public int DropDiamondPer; // 다이야 확률

        public void CopyFrom(MonsterStatsConfig other)
        {
            ID = other.ID;
            MonsterName = other.MonsterName;
            SizeValue = other.SizeValue;
            Class = other.Class;
            Hp = other.Hp;
            ATK = other.ATK;
            DFS = other.DFS;
            SPD = other.SPD;
            ATKSPD = other.ATKSPD;
            Exp = other.Exp;
            DropGold = other.DropGold;
            DropGoldPer = other.DropGoldPer;
            DropDiamond = other.DropDiamond;
            DropDiamondPer = other.DropDiamondPer;
        }
    }
}
