using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


namespace ProjectVS.UIs.UIBase
{
    public abstract class UIBase : MonoBehaviour
    {
        public virtual bool CanCloseWithEsc => true;

        public virtual void Init() { }

        public virtual void ShowImmediately()
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.one;
        }

        public virtual void HideImmediately()
        {
            gameObject.SetActive(false);
            transform.localScale = Vector3.one;
        }

        public virtual void AnimateShow(Action onComplete = null)
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;

            transform.DOScale(1f, 0.3f)
                .SetEase(Ease.OutBack)
                .OnComplete(() => onComplete?.Invoke());

            Debug.Log($"[UIBase] {gameObject.name}의 AnimateShow 호출 됨");
        }

        public virtual void AnimateHide(Action onComplete = null)
        {
            transform.DOScale(0f, 0.2f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    gameObject.SetActive(false);
                    onComplete?.Invoke();
                });
        }

        public virtual void FadeCanvasGroup(CanvasGroup group, float toAlpha, float duration, Action onComplete = null)
        {
            group.DOFade(toAlpha, duration).OnComplete(() => onComplete?.Invoke());
        }

        public virtual void PunchScale(float amount = 0.3f, float duration = 0.2f)
        {
            transform.DOPunchScale(Vector3.one * amount, duration, 10, 1f);
        }

        public virtual void ShakePosition(float duration = 0.3f, float strength = 10f)
        {
            transform.DOShakePosition(duration, strength);
        }
    }
}
