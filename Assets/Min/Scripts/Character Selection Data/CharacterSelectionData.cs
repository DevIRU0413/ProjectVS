using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectVS.CharacterSelectionData.CharacterSelectionData
{
    [System.Serializable]
    public class CharacterSelectionData
    {
        public int ID;
        public string Name;
        public int UniqueItemID;
        public string ItemName // 아이템 고유 번호 CSV를 어떻게 활용했는지 아직 몰라서 프로퍼티로 구현함
        {
            get
            {
                switch (UniqueItemID)
                {
                    case 10001: return "검";
                    case 10002: return "도끼";
                    case 10003: return "지팡이";
                    default: return "N/A";
                }
            }
        }
        public int Attack;
        public int Defense;
        public int HP;
        public int AttackSpeed;
        public int MoveSpeed;
        public string IllustPath;
        public string FlavorText;
    }
}
