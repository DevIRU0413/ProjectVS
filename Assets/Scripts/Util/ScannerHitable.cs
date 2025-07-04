using System;
using System.Collections.Generic;
using UnityEngine;
using ProjectVS.Unit;
using ProjectVS.Manager;
using ProjectVS.Util;
using ProjectVS;

public class ScannerHitable : MonoBehaviour
{
    [Header("Scanner References")]
    [SerializeField] private List<MonoBehaviour> scannerComponents; // IHitScanner 구현체들

    [Header("Hit Settings")]
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private bool canCrit = true;
    [SerializeField] private float critChance = 0.2f;
    [SerializeField] private float critMultiplier = 2f;
    [SerializeField] private float knockbackForce = 0.0f;
    [SerializeField, Min(1)] private int hitCount = 1;
    [SerializeField] private AudioClip hitClip;

    private UnitStats _unitStats;
    public Action OnHitStart;
    public Action OnHitEnd;

    public void SetOwner(UnitStats unitStats) => _unitStats = unitStats;

    public void PerformScanHit()
    {
        // 도중에 추가 가능하게, 특정 시점 스캐너를 뿌릴 수 있음
        var hitSet = new HashSet<GameObject>();
        foreach (var scannerComponent in scannerComponents)
        {
            if (!(scannerComponent is IHitScanner scanner))
            {
                Debug.LogWarning($"{scannerComponent.name} does not implement IHitScanner.");
                continue;
            }

            // 충돌 것들 추가
            var targets = scanner.Scan();
            foreach (var target in targets)
                hitSet.Add(target);
        }

        OnHitStart?.Invoke();

        foreach (var go in hitSet)
        {
            Damageable damageable = go.GetComponentInParent<Damageable>();
            if (damageable != null)
            {
                var info = CreateDamageInfo(go.transform.position);
                for (int i = 0; i < hitCount; i++)
                    damageable.ApplyDamage(info);

                if (hitClip != null)
                    AudioManager.Instance.PlaySfx(hitClip);
            }
        }

        OnHitEnd?.Invoke();
    }
    private DamageInfo CreateDamageInfo(Vector3 targetPos)
    {
        Vector2 dir = (targetPos - transform.position).normalized;
        DamageInfo info = new DamageInfo(baseDamage, dir)
        {
            IsCritical = canCrit && UnityEngine.Random.value < critChance,
            CriticalMultiplier = critMultiplier,
            KnockbackForce = knockbackForce
        };

        if (_unitStats != null)
            info.Amount = baseDamage + _unitStats.CurrentAtk;

        if (info.IsCritical)
            info.Amount *= critMultiplier;

        return info;
    }
}
