using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace ProjectVS.ItemData.ItemData
{
    public enum ItemRank
    {
        Sub = 0,
        Anvil = 1,
        Unique = 2,
        Composite = 3
    }

    public enum ItemType
    {
        Money = 0,
        Enforce = 1,
        Potion = 2,
        Attack = 3,
        Passive = 4
    }

    public enum ItemEffect
    {
        None = 0,
        Restore = 1,
        Swing = 2,
        Shot = 3,
        RandomShot = 4,
        Spin = 5,
        HPStatUP = 6,
        DefStatUP = 7,
        SpeedStatUP = 8,
        ThreeShot = 9,
        Tornado = 10,
        Throw = 11,
        Fiveway = 12,
        Bomb = 13,
        Double = 14,
        Eightway = 15
    }

    public enum ItemSetEffect
    {
        None = 0,
        TheftSet = 1,
        MagicianSet = 2,
        RobinSet = 3,
        NinjaSet = 4,
        KnightSet = 5,
    }

    [System.Serializable]
    public class ItemData
    {
        public int ID;
        public ItemRank ItemRank;
        public string ItemName;
        public ItemType ItemType;
        public float AttackSpeed;
        public ItemEffect Effect;
        public float EffectValue;
        public int CombineItem1;
        public int CombineItem2;
        public ItemSetEffect SetEffect;
        public int SetItemCount;
        public int MaxLevel;
        public int Price;
        public float Range;
        public float Size;
        public string Description;
        //public string ItemIconPath; TODO: 아이템 아이콘 경로(Resource폴더에 넣을) 추가해야 됨
    }
}
