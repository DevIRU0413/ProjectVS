using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectVS.UIs.PanelBehaviours.RecipePanelButtons
{
    public class RecipePanelButtons : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPanel;
        [SerializeField] private GameObject _setPanel;

        private bool _isItemPanelEnabled = true;

        private void Awake()
        {
            
        }

        private void OnEnable()
        {
            Time.timeScale = 0f;

            _isItemPanelEnabled = true;
            CheckWhatPanelEnabled();
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void OnClickItemButton()
        {
            _isItemPanelEnabled = true;
            CheckWhatPanelEnabled();
        }

        public void OnClickSetButton()
        {
            _isItemPanelEnabled = false;
            CheckWhatPanelEnabled();
        }


        public void CheckWhatPanelEnabled()
        {
            if (_isItemPanelEnabled)
            {
                _itemPanel.SetActive(true);
                _setPanel.SetActive(false);
            }
        }
    }
}
