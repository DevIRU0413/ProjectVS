using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;


namespace ProjectVS.UIs.PanelBehaviours.SettingsPanelButtons
{
    public class SettingsPanelButtons : MonoBehaviour
    {
        private void OnEnable()
        {
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void OnClickESCButton()
        {
            UIManager.Instance.CloseTopPanel();
        }
    }
}
