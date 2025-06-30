using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectVS.CharacterImages.CostumeSO
{
    [CreateAssetMenu(menuName = "Create New Costume")]
    public class CostumeSO : ScriptableObject
    {
        public string CostumeName;
        public string CostumeDescription;
        public Sprite CostumeSprite;
        public Sprite FullScene;
        // 추가로 아이콘 스프라이트도 있어야 될 듯
        public Sprite IconSprite;
        public bool IsUnlocked = false;
        public bool IsEquipped = false;
        public int CostumeDialogueID;
    }
}
