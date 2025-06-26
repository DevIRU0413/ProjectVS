using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ProjectVS.Dialogue.TextEffect.DialogueTextTyper;
using ProjectVS.Utils.UIManager;
using DialogueManagerClass = ProjectVS.Dialogue.DialogueManager.DialogueManager;

namespace ProjectVS.UIs.PanelBehaviours.EventPanelButtons
{
    public class EventPanelButtons : MonoBehaviour
    {
        public void OnClickESCButton()
        {
            // TODO: 만약 이 씬이 최하단 씬이 아니라는 보장이 없다면 ForceCloseTopPanel 사용
            UIManager.Instance.CloseTopPanel();
        }

        public void OnClickLogButton()
        {
            UIManager.Instance.Hide("Event Panel");
            UIManager.Instance.Show("Log Panel");
        }
    }
}
