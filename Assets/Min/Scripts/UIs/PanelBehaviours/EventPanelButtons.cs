using System.Collections;
using System.Collections.Generic;

using ProjectVS.Utils.UIManager;

using UnityEngine;


namespace ProjectVS.UIs.PanelBehaviours.EventPanelButtons
{
    public class EventPanelButtons : MonoBehaviour
    {
        public void OnClickESCButton()
        {
            // TODO: 만약 이 씬이 최하단 씬이 아니라는 보장이 없다면 ForceCloseTopPanel 사용
            UIManager.Instance.CloseTopPanel();
        }
    }
}
