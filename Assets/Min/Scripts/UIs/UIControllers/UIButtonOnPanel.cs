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
            StartCoroutine(DelayThenAnimateIn(onComplete)); // Show 호출 전에 gameObject가 먼저 setActive true 되어 애니메이션이 보이지 않아 1프레임 대기

            Debug.Log($"[UIButtonOnPanel] {gameObject.name}의 AnimateShow 호출 됨");
        }

        // 내부 코루틴으로 패널이 먼저 뜨고 나서 버튼 그룹들 등장
        private IEnumerator DelayThenAnimateIn(Action onComplete)
        {
            // 1 프레임 대기
            yield return null;

            // 이제 버튼 UI 등장 애니메이션 실행
            _panelController?.AnimateIn(onComplete);
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
