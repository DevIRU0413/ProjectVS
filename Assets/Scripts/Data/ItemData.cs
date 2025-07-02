namespace ProjectVS.Data
{
    [System.Serializable]
    public class ItemData
    {
        // 기본 정보
        public int ItemID;
        public string ItemName;
        public ItemRank ItemRank;
        public ItemType ItemType;
        public ItemEffect ItemEffect;

        // 스탯
        public float AttackSpeed;
        public float ItemEffectValue;
        public int MaxLevel;
        public int Price;
        public float Range;
        public float Size;

        // 조합
        public int CombineItemID1;
        public int CombineItemID2;

        // 세트 효과
        public ItemSetEffect SetEffect;
        public int SetEffectRequiredCount;

        // 아이콘
        public string ItemIconPath;

        // 설명
        public string Description;
    }
}
