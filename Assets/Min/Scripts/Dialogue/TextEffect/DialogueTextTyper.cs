using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TextTyperBaseClass = ProjectVS.Dialogue.TextEffect.TextTyperBase.TextTyperBase;


namespace ProjectVS.Dialogue.TextEffect.DialogueTextTyper
{
    public class DialogueTextTyper : TextTyperBaseClass
    {
        protected override void Awake()
        {
            base.Awake();
            if (_contentText == null)
            {
                _contentText = GetComponent<TMPro.TMP_Text>();
            }
        }
    }
}
