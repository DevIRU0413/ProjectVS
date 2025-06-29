using System.Collections;
using System.Collections.Generic;

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
        }

        public void OnClickRecipeButton()
        {
            UIManager.Instance.Hide("Pause Buttons Panel");
            UIManager.Instance.Show("Recipe Panel");
        }

        public void OnClickGoToMainMenuButton()
        {
            SceneManager.LoadScene(1); // 메인메뉴가 1번 씬이라고 가정
        }
    }
}
