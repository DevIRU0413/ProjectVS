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
        public Sprite IconSprite;
        public bool IsUnlocked = false;
        public bool IsEquipped = false;
        public int CostumeDialogueID;
        public int Price;
    }
}
