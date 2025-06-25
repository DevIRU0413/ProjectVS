using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectVS.Dialogue.ChoiceData
{
    [System.Serializable]
    public class ChoiceData : MonoBehaviour
    {
        public int ID;
        public int NextDialogueID;
        public string ChoiceText1;
        public string ChoiceText2;
    }
}

