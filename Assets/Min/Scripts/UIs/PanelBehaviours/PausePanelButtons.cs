using System.Collections;
using System.Collections.Generic;

using ProjectVS.Manager;
using ProjectVS.Utils.UIManager;

using UnityEngine;
using UnityEngine.SceneManagement;


namespace ProjectVS.UIs.PanelBehaviours.CharacterIndicatorPausePanelButtons
{
    public class PausePanelButtons : MonoBehaviour
    {
        private void OnEnable()
        {
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void OnClickResumeGame()
        {
            UIManager.Instance.ForceCloseTopPanel();
        }

        public void OnClickSettingsButton()
        {
            UIManager.Instance.Hide("Pause Buttons Panel");
            UIManager.Instance.Show("Settings Panel");

            Debug.Log($"[PausePanelButtons] Settings Button 패널 호출");
        }

        public void OnClickRecipeButton()
        {
            UIManager.Instance.Hide("Pause Buttons Panel");
            UIManager.Instance.Show("Recipe Panel");

            Debug.Log($"[PausePanelButtons] Recipe Button 패널 호출");
        }

        public void OnClickGoToMainMenuButton()
        {
            SceneLoader.Instance.LoadSceneAsync(SceneID.MainMenuScene);
        }
    }
}
