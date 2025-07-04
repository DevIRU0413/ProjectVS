using ProjectVS.JDW;

using UnityEngine;

public class ItemEffect_Bomb : MonoBehaviour
{

    private bool _isBomb;
    private float _damage;
    private float _radius;

    public void Init(float damage, float radius)
    {
        _damage = damage;
        _radius = radius;
    }

    private void OnDestroy()
    {
        if (_isBomb )
            return;

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Monster"))
            {
                hit.GetComponent<Monster>()?.TakeDamage(_damage);
                Debug.Log($"[Bomb] {hit.name} 피해 {_damage}");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("Monster"))
            {
                hit.GetComponent<Monster>()?.TakeDamage(_damage);
                Debug.Log($"대상 : {hit.name} / 데미지 : {_damage}");
            }
        }

        _isBomb = true;
        Destroy(gameObject);
    }
}
