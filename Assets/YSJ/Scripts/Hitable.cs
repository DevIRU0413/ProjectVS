using System;

using ProjectVS;
using ProjectVS.Util;

using UnityEngine;

// 공격 할 수 있음.
public class Hitable : MonoBehaviour
{
    [SerializeField] private Collider2D _hitCollider;
    [SerializeField] private LayerMask _hitLayer;

    // 최소 수치가 0인 이유, 차후 충돌 시 > 버프 또는 디버프 관련 요청을 위함
    [SerializeField, Min(0)] private int hitCount = 1;

    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private bool canCrit = true;
    [SerializeField] private float critChance = 0.2f;
    [SerializeField] private float critMultiplier = 2f;
    [SerializeField] private float knockbackForce = 5f;

    private Collider2DAction _colliderAction;

    public Action OnHitEvent; // 충돌 되었을 때, 추가 처리할 이벤트
    public Action OnHitEnd; // 충돌 되었을 때, 추가 처리할 이벤트

    /// <summary>
    /// 외부 컴포넌트에서 액션 추가용(Start 이상 시점에서 사용할 것)
    /// </summary>
    public Collider2DAction ColliderAction => _colliderAction;

    private void Awake()
    {
        if (_hitCollider == null)
            _hitCollider = this.GetComponentInChildren<Collider2D>();
        _colliderAction = _hitCollider.gameObject.GetOrAddComponent<Collider2DAction>();

        _colliderAction.OnTriggerEnterAction -= HitTargget;
        _colliderAction.OnTriggerEnterAction += HitTargget;
    }

    // Trigger
    public virtual void HitTargget(Collider2D collider)
    {
        // 비트 연산 진행 레이어 검출
        int layerCheck = 1 << collider.gameObject.layer  & _hitLayer;
        if (layerCheck != _hitLayer) return;

        if (collider.TryGetComponent<Damagable>(out var damagable))
        {
            Vector2 dir = (collider.transform.position - transform.position).normalized;
            DamageInfo info = new DamageInfo(baseDamage, dir)
            {
                IsCritical = canCrit && UnityEngine.Random.value < critChance,
                CriticalMultiplier = critMultiplier,
                KnockbackForce = knockbackForce
            };

            if (info.IsCritical)
                info.Amount *= info.CriticalMultiplier;

            for (int i = 0; i < hitCount; i++)
            {
                damagable.ApplyDamage(info);
                OnHitEvent?.Invoke();
            }

            OnHitEnd?.Invoke();
        }
    }
}
