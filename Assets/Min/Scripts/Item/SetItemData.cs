using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectVS.Item.SetItemData
{
    [System.Serializable]
    public class SetItemData : MonoBehaviour
    {
        public int ID;
        public string Name;
        public int SetEffect;
        public int SetEffectValue;
        public int[] BaseItemID;
        public string IllustPath;
    }
}
