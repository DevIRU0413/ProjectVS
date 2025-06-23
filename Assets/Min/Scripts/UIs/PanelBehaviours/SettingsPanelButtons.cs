using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;


namespace ProjectVS.UIs.PanelBehaviours.SettingsPanelButtons
{
    public class SettingsPanelButtons : MonoBehaviour
    {
        public void OnClickESCButton()
        {
            UIManager.Instance.CloseTopPanel();
        }
    }
}
