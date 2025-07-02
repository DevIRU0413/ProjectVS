using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHiteEffect : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Color _color;
    private Coroutine _hitRoutine;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _color = _spriteRenderer.color;
    }
    private void PlayerHitEffect()
    {
        if (_hitRoutine != null)
        {
            HitFlasRoutine();
        }
    }
    private IEnumerator HitFlasRoutine()
    {
        _spriteRenderer.color = new Color(1, 0.5f, 0.5f);
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = _color;
    }
}
