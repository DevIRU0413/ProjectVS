using System.Collections;
using System.Collections.Generic;
using Kino;

using UnityEngine;

namespace ProjectVS.UIs.CutSceneEffect.GlitchEffector
{
    public class GlitchEffector : MonoBehaviour
    {
        [Header("글리치 이펙트 설정")]
        [SerializeField] private DigitalGlitch _digitalGlitch;
        [SerializeField] private float _glitchEffectDuration = 2f;

        public void OnClickGlitchEffectImage()
        {
            StartCoroutine(IE_GlitchEffect());
        }

        private IEnumerator IE_GlitchEffect()
        {
            _digitalGlitch.intensity = 0.5f;

            yield return new WaitForSeconds(_glitchEffectDuration);

            _digitalGlitch.intensity = 0f;
        }
    }
}


