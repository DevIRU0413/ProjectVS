using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

using UnityEngine;
using System;


namespace ProjectVS.UIs.UIBase.UIButtonAnimator
{
    public enum SlideDirection
    {
        Horizontal,
        Vertical
    }

    public class UIButtonAnimator : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> _buttons;

        [Header("방향 설정")]
        [SerializeField] private SlideDirection _slideDirection = SlideDirection.Horizontal;

        [Header("슬라이드 위치 설정")]
        [SerializeField] private float _originOffset = 0f;
        [SerializeField] private float _moveInOffset = -300f;
        [SerializeField] private float _moveOutOffset = -300f;

        [Header("애니메이션 시간 설정")]
        [SerializeField] private float _moveInDuration = 0.5f;
        [SerializeField] private float _fadeInDuration = 0.5f;
        [SerializeField] private float _moveOutDuration = 0.5f;
        [SerializeField] private float _fadeOutDuration = 0.5f;
        [SerializeField] private float _interval = 0.05f;

        [Header("Ease 설정")]
        [SerializeField] private Ease _moveInEase = Ease.OutBack;
        [SerializeField] private Ease _moveOutEase = Ease.InBack;

        private Vector2 GetOffsetPosition(RectTransform rect, float offset)
        {
            return _slideDirection == SlideDirection.Horizontal
                ? new Vector2(offset, rect.anchoredPosition.y)
                : new Vector2(rect.anchoredPosition.x, offset);
        }

        public IEnumerator AnimateIn(Action onComplete = null)
        {
            foreach (var rect in _buttons)
            {
                rect.gameObject.SetActive(true);
                rect.anchoredPosition = GetOffsetPosition(rect, _moveInOffset);
                rect.DOAnchorPos(GetOffsetPosition(rect, _originOffset), _moveInDuration)
                    .SetEase(_moveInEase);

                CanvasGroup cg = rect.GetComponent<CanvasGroup>();
                if (cg != null)
                    cg.DOFade(1f, _fadeInDuration);

                yield return new WaitForSeconds(_interval);
            }

            onComplete?.Invoke();
        }

        public IEnumerator AnimateOut(Action onComplete = null)
        {
            foreach (var rect in _buttons)
            {
                rect.DOAnchorPos(GetOffsetPosition(rect, _moveOutOffset), _moveOutDuration)
                    .SetEase(_moveOutEase);

                CanvasGroup cg = rect.GetComponent<CanvasGroup>();
                if (cg != null)
                    cg.DOFade(0f, _fadeOutDuration);

                yield return new WaitForSeconds(_interval);
            }

            foreach (var rect in _buttons)
            {
                rect.gameObject.SetActive(false);
            }

            onComplete?.Invoke();
        }

        public void ShowInstant()
        {
            foreach (var rect in _buttons)
            {
                rect.gameObject.SetActive(true);
                rect.anchoredPosition = GetOffsetPosition(rect, _originOffset);

                CanvasGroup cg = rect.GetComponent<CanvasGroup>();
                if (cg != null)
                    cg.alpha = 1f;
            }
        }

        public void HideInstant()
        {
            foreach (var rect in _buttons)
            {
                rect.gameObject.SetActive(false);
            }
        }
    }
}
