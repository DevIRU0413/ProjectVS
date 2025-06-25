using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ProjectVS.Utils.UIManager;
using UIPanelClass = ProjectVS.UIs.UIControllers.UIPanel.UIPanel;


namespace ProjectVS.UIs.UIBase.SceneUIRegistrar
{
    public class SceneUIRegistrar : MonoBehaviour
    {
        private void Awake()
        {
            foreach (var panel in GetComponentsInChildren<UIPanelClass>(true))
            {
                string key = panel.name;
                GameObject go = panel.gameObject;

                UIManager.Instance.RegisterPanel(key, go);

                var uiBase = go.GetComponent<UIBase>();
                uiBase?.Init();
            }
        }
    }
}
