using UnityEngine;

namespace ProjectVS.Item
{
    [CreateAssetMenu(fileName = "NewItemData", menuName = "ItemSO")]
    [System.Serializable]
    public class ItemData : ScriptableObject
    {
        [Header("아이템 정보")]
        [field: SerializeField] public int ItemID;
        [field: SerializeField] public ItemRank ItemRank;
        [field: SerializeField] public string ItemName;
        [field: SerializeField] public ItemType ItemType;

        [Header("공격 속도 및 효과")]
        [field: SerializeField] public float ItemAtkSpeed;
        [field: SerializeField] public ItemEffect ItemEffect;
        [field: SerializeField] public int ItemEffectValue;

        [Header("조합 재료")]
        [field: SerializeField] public int ItemAddID1;
        [field: SerializeField] public int ItemAddID2;

        [Header("세트 정보")]
        [field: SerializeField] public ItemSetEffect ItemSetEffect;
        [field: SerializeField] public int ItemSetNum;

        [Header("아이템 최대 레벨 및 가격")]
        [field: SerializeField] public int ItemMaxLevel;
        [field: SerializeField] public int ItemValue;

        [Header("공격 아이템 전용")]
        [field: SerializeField] public int ItemRange;
        [field: SerializeField] public float ItemSize;

        [Header("아이콘")]
        [field: SerializeField] public Sprite ItemIcon;

        /// <summary>
        /// 조합 아이템 여부 확인
        /// </summary>
        public bool IsComposite => ItemRank == ItemRank.Composite;

        /// <summary>
        /// 세트 효과 여부 확인
        /// </summary>
        public bool HasSetEffect => ItemSetNum > 0;
    }
}
