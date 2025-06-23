using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UIButtonAnimatorClass = ProjectVS.UIs.UIBase.UIButtonAnimator.UIButtonAnimator;


namespace ProjectVS.UIs.UIBase.UIButtonGroupController
{
    public class UIButtonGroupController : MonoBehaviour
    {
        [SerializeField] private List<UIButtonAnimatorClass> _group;

        public void AnimateIn(Action onComplete = null)
        {
            StartCoroutine(AnimateGroupsIn(onComplete));
        }

        public void AnimateOut(Action onComplete = null)
        {
            StartCoroutine(AnimateGroupsOut(onComplete));
        }

        public void ShowInstant()
        {
            foreach (var btn in _group)
                btn.ShowInstant();
        }

        public void HideInstant()
        {
            foreach (var btn in _group)
                btn.HideInstant();
        }

        private IEnumerator AnimateGroupsIn(Action onComplete = null)
        {
            foreach (var btn in _group)
            {
                yield return StartCoroutine(btn.AnimateIn());
            }

            onComplete?.Invoke();
        }

        private IEnumerator AnimateGroupsOut(Action onComplete = null)
        {
            for (int i = _group.Count - 1; i >= 0; i--) // 역순으로 닫기
            {
                yield return StartCoroutine(_group[i].AnimateOut());
            }

            onComplete?.Invoke();
        }
    }
}
