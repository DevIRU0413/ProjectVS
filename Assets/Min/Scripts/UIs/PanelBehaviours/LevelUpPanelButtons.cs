using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;
using TMPro;


namespace ProjectVS.UIs.PanelBehaviours.LevelUpPanelButtons
{
    public class LevelUpPanelButtons : MonoBehaviour
    {
        [SerializeField] private GameObject selectPanel;
        [SerializeField] private TMP_Text _hideUIText;

        private bool _isHiding = false;

        private void OnEnable()
        {
            Time.timeScale = 0f;
            _isHiding = false;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void OnToggleHideButton()
        {
            _isHiding = !_isHiding;

            if (_isHiding)
            {
                selectPanel.SetActive(false);
                _hideUIText.text = "UI 켜기";
            }
            else
            {
                selectPanel.SetActive(true);
                _hideUIText.text = "UI 끄기";
            }
        }
    }
}
