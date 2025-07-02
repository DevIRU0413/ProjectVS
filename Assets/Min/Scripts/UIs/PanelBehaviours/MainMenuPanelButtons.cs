using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;


namespace ProjectVS.UIs.PanelBehaviours.MainMenuPanel
{
    public class MainMenuPanelButtons : MonoBehaviour
    {
        private void Start()
        {
            UIManager.Instance.ShowAsFirst("Main Menu Panel");
        }

        public void OnClickStartButton()
        {
            UIManager.Instance.Hide("Main Menu Panel");
            UIManager.Instance.Show("Control File Panel");
        }

        public void OnClickSettingsButton()
        {
            UIManager.Instance.Hide("Main Menu Panel");
            UIManager.Instance.Show("Settings Panel");
        }

        public void OnClickGalleryButton()
        {
            UIManager.Instance.Hide("Main Menu Panel");
            UIManager.Instance.Show("Gallery Panel");
        }

        public void OnClickExitButton()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
