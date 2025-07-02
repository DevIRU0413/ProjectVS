using System.Collections;
using System.Collections.Generic;

using DG.Tweening;

using UnityEngine;


namespace ProjectVS.NPC.EmojiBehaviour
{
    public class EmojiBehaviour : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("이모지 사라짐 모션 설정")]
        [SerializeField, Range(0f, 10f)] private float _vanishDuration = 2f;

        private void Awake()
        {
            if (_spriteRenderer == null)
            {
                _spriteRenderer = GetComponent<SpriteRenderer>();
            }
        }

        private void OnEnable()
        {
            Color color = _spriteRenderer.color;
            color.a = 1f;
            _spriteRenderer.color = color;

            _spriteRenderer.DOFade(0f, _vanishDuration).OnComplete(() => gameObject.SetActive(false));
        }
    }
}
