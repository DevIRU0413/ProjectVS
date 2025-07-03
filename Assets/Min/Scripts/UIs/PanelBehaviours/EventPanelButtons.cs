using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;
using ProjectVS.Dialogue.DialogueManager;
using ProjectVS.Dialogue.DialogueManagerR;

namespace ProjectVS.UIs.PanelBehaviours.EventPanelButtons
{
    public class EventPanelButtons : MonoBehaviour
    {
        private void OnEnable()
        {
            //DialogueManager.Instance.IsClosed = false;
            DialogueManagerR.Instance.IsClosed = false;
            Time.timeScale = 0f;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
        }

        public void OnClickESCButton()
        {
            // TODO: 만약 이 씬이 최하단 씬이 아니라는 보장이 없다면 ForceCloseTopPanel 사용
            UIManager.Instance.CloseTopPanel();
        }

        public void OnClickLogButton()
        {
            //DialogueManager.Instance.IsAutoMode.Value = false;
            DialogueManagerR.Instance.IsAutoMode.Value = false;

            UIManager.Instance.Hide("Event Panel");
            UIManager.Instance.Show("Log Panel");
        }

        public void OnClickNextButton()
        {
            DialogueManagerR.Instance.Next();
        }
    }
}
