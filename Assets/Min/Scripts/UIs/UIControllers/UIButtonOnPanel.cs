using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;

using UIButtonGroupControllerClass = ProjectVS.UIs.UIBase.UIButtonGroupController.UIButtonGroupController;


namespace ProjectVS.UIs.UIBase.UIButtonOnPanel
{
    public class UIButtonOnPanel : UIBase
    {
        [SerializeField] private UIButtonGroupControllerClass _panelController;

        public override void Init()
        {
            if (_panelController == null)
            {
                _panelController = GetComponent<UIButtonGroupControllerClass>();
            }
        }

        public override void AnimateShow(Action onComplete = null)
        {
            gameObject.SetActive(true);
            _panelController?.AnimateIn(onComplete);

            Debug.Log($"[UIButtonOnPanel] {gameObject.name}의 AnimateShow 호출 됨");
        }

        public override void ShowImmediately()
        {
            gameObject.SetActive(true);
            _panelController?.ShowInstant();
        }

        public override void AnimateHide(Action onComplete = null)
        {
            _panelController?.AnimateOut(() =>
            {
                gameObject.SetActive(false);
                onComplete?.Invoke();
            });
        }

        public override void HideImmediately()
        {
            _panelController?.HideInstant();
            gameObject.SetActive(false);
        }
    }
}
