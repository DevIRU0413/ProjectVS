using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TextTyperBaseClass = ProjectVS.Dialogue.TextEffect.TextTyperBase.TextTyperBase;


namespace ProjectVS.Dialogue.TextEffect.DialogueTextTyper
{
    public class DialogueTextTyper : TextTyperBaseClass
    {
        private void Awake()
        {
            if (_text == null)
            {
                _text = GetComponent<TMPro.TMP_Text>();
            }
        }
    }
}
