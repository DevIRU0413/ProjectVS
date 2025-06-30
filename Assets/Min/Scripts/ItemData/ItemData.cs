using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectVS.ItemData.ItemData
{
    [System.Serializable]
    public class ItemData
    {
        public int ID;
        public string ItemRank;
        public string ItemName;
        public string ItemType;
        public float AttackSpeed;
        public string Effect;
        public float EffectValue;
        public int CombineItem1;
        public int CombineItem2;
        public string SetEffect;
        public int SetItemCount;
        public int MaxLevel;
        public int Price;
        public float Range;
        public float Size;
        public string Description;
        //public string ItemIconPath; TODO: 아이템 아이콘 경로(Resource폴더에 넣을) 추가해야 됨
    }
}
