using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectVS.Item.SetItemData
{
    [System.Serializable]
    public class SetItemData
    {
        public int ID;
        public string SetName;
        public int SetEffect;
        public float[] SetValue;
        public int[] SetItemID;
        public string SetItemICON;
        public Sprite SetIcon;
        public string SetEffectText;
    }
}
