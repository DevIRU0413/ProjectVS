using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiteEffect : MonoBehaviour
{
    [SerializeField] private Color _hitColor = new Color(1f, 0.5f, 0.5f);
    [SerializeField] private float _flashDuration = 0.1f;

    private SpriteRenderer _spriteRenderer;
    private Color _color;
    private Coroutine _hitRoutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _color = _spriteRenderer.color;
    }
    public void PlayerHitEffect()
    {
        if (_hitRoutine != null)
        {
            StopCoroutine(_hitRoutine);
        }
        _hitRoutine = StartCoroutine(HitFlashRoutine());
    }
    public IEnumerator HitFlashRoutine()
    {
        _spriteRenderer.color = _hitColor;
        yield return new WaitForSeconds(_flashDuration);
        _spriteRenderer.color = _color;
        _hitRoutine = null;
    }
}
