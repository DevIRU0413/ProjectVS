using System;

using ProjectVS;
using ProjectVS.Manager;
using ProjectVS.Unit;
using ProjectVS.Util;

using UnityEngine;

// 공격 할 수 있음.
public class Hitable : MonoBehaviour
{
    [SerializeField] private Collider2D _hitCollider;
    [SerializeField] protected LayerMask _hitLayer;

    // 최소 수치가 0인 이유, 차후 충돌 시 > 버프 또는 디버프 관련 요청을 위함
    [SerializeField, Min(0)] protected int _hitCount = 1;

    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private bool canCrit = true;
    [SerializeField] private float critChance = 0.2f;
    [SerializeField] private float critMultiplier = 2f;
    [SerializeField] private float knockbackForce = 0.0f;

    [Header("audio Info")]
    [SerializeField] protected AudioClip _hitClip;

    protected float _hitTime = 0.0f;
    protected UnitStats _unitStats;

    // 충돌 체크
    protected Collider2DAction _colliderAction;

    // 외부 이벤트 추가 가능
    public Action OnEnterHitEvent;
    public Action OnEnterHitEnd;

    private void Awake() => Init();

    protected virtual void Init()
    {
        if (_hitCollider == null)
            _hitCollider = this.GetComponentInChildren<Collider2D>();
        _colliderAction = _hitCollider.gameObject.GetOrAddComponent<Collider2DAction>();

        _colliderAction.OnTriggerEnterAction -= HitTriggerEnter;
        _colliderAction.OnTriggerEnterAction += HitTriggerEnter;
    }

    // Trigger
    protected virtual void HitTriggerEnter(Collider2D collider)
    {
        Hit(collider);
    }

    protected void Hit(Collider2D collider)
    {
        // 비트 연산 진행 레이어 검출
        int layerCheck = 1 << collider.gameObject.layer  & _hitLayer;
        if (layerCheck != _hitLayer) return;

        if (collider.TryGetComponent<Damagable>(out var damagable))
        {
            if (_hitClip != null)
                AudioManager.Instance.PlaySfx(_hitClip);

            DamageInfo info = GetDamageInfo(collider);
            for (int i = 0; i < _hitCount; i++)
            {
                damagable.ApplyDamage(info);
                OnEnterHitEvent?.Invoke();
            }

            OnEnterHitEnd?.Invoke();
        }
    }

    // ETC
    public void SetOnwer(UnitStats unitStats)
    {
        _unitStats = unitStats;
    }
    protected DamageInfo GetDamageInfo(Collider2D collider)
    {
        Vector2 dir = (collider.transform.position - transform.position).normalized;
        DamageInfo info = new DamageInfo(baseDamage, dir)
        {
            IsCritical = canCrit && UnityEngine.Random.value < critChance,
            CriticalMultiplier = critMultiplier,
            KnockbackForce = knockbackForce
        };

        if (_unitStats != null)
            info.Amount = baseDamage + _unitStats.CurrentAtk;

        if (info.IsCritical)
            info.Amount *= info.CriticalMultiplier;

        return info;
    }
}
