using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;


namespace ProjectVS.UIs.PanelBehaviours.CharacterSelectionPanelButtons
{
    public class CharacterSelectionPanelButtons : MonoBehaviour
    {
        public void OnClickESCButton()
        {
            UIManager.Instance.CloseTopPanel();
        }
    }
}
