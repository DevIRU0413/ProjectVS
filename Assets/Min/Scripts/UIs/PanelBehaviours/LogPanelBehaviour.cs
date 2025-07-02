using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Min.UIs.PanelBehaviours.LogPanelBehaviour
{
    public class LogPanelBehaviour : MonoBehaviour
    {
        private void OnEnable()
        {
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }
    }
}
