using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectVS.Dialogue.ChoiceData
{
    [System.Serializable]
    public class ChoiceData
    {
        public int ID;
        public int NextDialogueID;
        public string ChoiceText1;
        public string ChoiceText2;

        public string CharacterName => "주인공";
    }
}

