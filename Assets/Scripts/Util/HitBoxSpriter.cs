using System.Collections;

using UnityEngine;

public class HitBoxSpriter : MonoBehaviour
{
    [Range(0, 1.0f)]public float MinAlpha;
    [Range(0, 1.0f)]public float MaxAlpha;
    [Min(0.0f)] public float changeTime = 1.0f;
    public  bool forward = true;

    public Color useColor;
    public SpriteRenderer spriteRenderer;

    private Coroutine _coroutine;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = useColor;
    }

    private void OnEnable()
    {
        _coroutine = StartCoroutine(ChangeColor());
    }

    private void OnDisable()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);
    }

    private IEnumerator ChangeColor()
    {
        float timer = 0f;
        Color color = useColor;

        while (true)
        {
            yield return null;

            timer += Time.deltaTime;
            float t = timer / changeTime;
            t = Mathf.Clamp01(t); // ensure 0~1

            float alpha = forward ? Mathf.Lerp(MinAlpha, MaxAlpha, t) : Mathf.Lerp(MaxAlpha, MinAlpha, t);

            color.a = alpha;
            spriteRenderer.color = color;

            if (t >= 1f)
            {
                timer = 0f;
                forward = !forward; 
            }
        }
    }
}
