using ProjectVS.ItemData.ItemData;

using UnityEngine;

namespace ProjectVS.Data.Player
{

    [CreateAssetMenu(fileName = "NewItem", menuName = "Item/ItemData")]
    public class ItemDataSO : ScriptableObject
    {
        [Header("기본 정보")]
        public int ItemID;
        public string ItemName;
        public ItemRank ItemRank;
        public ItemType ItemType;
        public ItemEffect ItemEffect;

        [Header("스탯")]
        public float ItemAtkSpeed;
        public int ItemEffectValue;
        public int MaxLevel;
        public int Price;
        public int Range;
        public float Size;

        [Header("조합")]
        public int CombineItemID1;
        public int CombineItemID2;

        [Header("세트 효과")]
        public ItemSetEffect SetEffect = ItemSetEffect.None;
        public int SetEffectRequiredCount;

        [Header("아이콘")]
        public Sprite ItemIcon;

        [TextArea] public string Description;
    }
}
