using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

using UnityEngine;
using UnityEngine.UI;

namespace ProjectVS.CutSceneEffectors.FadeInAndOutEffector
{
    public enum EffectType
    {
        FadeIn,
        FadeOut,
        FadeInAndOut
    }

    public class FadeInAndOutEffector : MonoBehaviour
    {
        [SerializeField] private Image _image;

        [Header("Fade In/Out 설정")]
        [SerializeField] private EffectType _effectType;
        [SerializeField] private float _fadeDuration = 1f;

        private void Awake()
        {
            if (_image == null)
            {
                _image = GetComponent<Image>();
                if (_image == null)
                {
                    Debug.LogError("[FadeInAndOutEffector] Image 컴포넌트를 찾을 수 없습니다.");
                }
            }
        }

        private void OnEnable()
        {
            switch (_effectType)
            {
                case EffectType.FadeIn:
                    FadeIn();
                    break;
                case EffectType.FadeOut:
                    FadeOut();
                    break;
                case EffectType.FadeInAndOut:
                    FadeIn(() => FadeOut());
                    break;
            }
        }

        private void FadeIn(Action onComplete = null)
        {
            SetAlpha(0f);
            _image.DOFade(1f, _fadeDuration).OnComplete(() => onComplete?.Invoke());
        }

        private void FadeOut(Action onComplete = null)
        {
            SetAlpha(1f);
            _image.DOFade(0f, _fadeDuration).OnComplete(() => onComplete?.Invoke());
        }

        private void SetAlpha(float alpha)
        {
            Color color = _image.color;
            color.a = alpha;
            _image.color = color;
        }
    }
}
