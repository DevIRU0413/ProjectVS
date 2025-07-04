using System;
using System.Collections.Generic;
using UnityEngine;
using ProjectVS.Unit;
using ProjectVS.Util;
using ProjectVS.Manager;
using ProjectVS;

public class ScanHitable : MonoBehaviour
{
    [Header("Scan Settings")]
    [SerializeField] private float scanRadius = 1.0f;
    [SerializeField] private LayerMask targetMask;
    [SerializeField] private bool useBoxScan = false;
    [SerializeField] private Vector2 boxSize = Vector2.one;
    [SerializeField] private Vector2 boxOffset = Vector2.zero;
    [SerializeField] private float boxAngle = 0f;

    [Header("Hit Settings")]
    [SerializeField] private float baseDamage = 10f;
    [SerializeField] private bool canCrit = true;
    [SerializeField] private float critChance = 0.2f;
    [SerializeField] private float critMultiplier = 2f;
    [SerializeField] private float knockbackForce = 0.0f;
    [SerializeField, Min(1)] private int hitCount = 1;

    [Header("Audio")]
    [SerializeField] private AudioClip hitClip;

    private Collider2D[] _buffer = new Collider2D[20];
    private UnitStats _unitStats;

    public Action OnHitStarted;
    public Action OnHitEnded;

    public void SetOwner(UnitStats stats) => _unitStats = stats;

    public void PerformScanHit()
    {
        List<GameObject> targets = useBoxScan ?
            OverlapScanUtility.BoxScan(transform.position, boxOffset, boxSize, boxAngle, _buffer, targetMask, FilterTarget) :
            OverlapScanUtility.CircleScan(scanRadius, transform.position, targetMask, _buffer, FilterTarget);

        OnHitStarted?.Invoke();

        foreach (var target in targets)
        {
            if (target.TryGetComponent<Damageable>(out var damageable))
            {
                DamageInfo info = CreateDamageInfo(target.transform.position);
                for (int i = 0; i < hitCount; i++)
                    damageable.ApplyDamage(info);

                if (hitClip != null)
                    AudioManager.Instance.PlaySfx(hitClip);
            }
        }

        OnHitEnded?.Invoke();
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

    private bool FilterTarget(GameObject go)
    {
        // 확장 가능: 예를 들어 태그 체크 등
        return go.activeInHierarchy;
    }
}
