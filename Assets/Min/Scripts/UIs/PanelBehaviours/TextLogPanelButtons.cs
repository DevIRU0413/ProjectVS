using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace ProjectVS.UIs.PanelBehaviours.TextLogPanelButtons
{
    public class TextLogPanelButtons : MonoBehaviour
    {
        public void OnClickESCButton()
        {
            gameObject.SetActive(false);
        }
    }
}
