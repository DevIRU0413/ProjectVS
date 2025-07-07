using System;

using ProjectVS.Monster;
using ProjectVS.Util;

using UnityEngine;

// 공격 할 수 있음.
[RequireComponent(typeof(Hitable))]
public class HitableObjectDestroy : MonoBehaviour, IPoolable
{
    [SerializeField] private bool _isPoolableObject = false; // 충돌 시, 소멸을 풀링으로 처리할 것인지 아닌지
    [SerializeField] private int _destroyCollisionCount = 1; // 몇번 충돌 시 소멸하는지

    private Hitable _hitableCmp;
    private int _hitedCount = 0;

    public Action OnSpawn { get; set; }
    public Action OnDespawn { get; set; }

    // 초기화
    private void Awake() => Init();
    protected void Init()
    {
        _hitableCmp = gameObject.GetOrAddComponent<Hitable>();

        _hitedCount = 0;

        _hitableCmp.OnEnterHitEnd -= HitCheck;
        _hitableCmp.OnEnterHitEnd += HitCheck;
    }

    // 활성화 시, Destory될 오브젝트라면 해당 기능 필요없음.
    /*private void OnEnable()
    {
        _hitedCount = 0;
    }*/

    // hit 시
    private void HitCheck()
    {
        _hitedCount++;
        DestroyCheck();
    }

    // hit
    private void DestroyCheck()
    {
        Debug.Log($"{_hitedCount} > {_destroyCollisionCount}");
        if(_hitedCount >= _destroyCollisionCount)
        {
            if (_isPoolableObject)
            {
                PoolManager.Instance.Despawn(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }

    public void OnSpawned()
    {
        _hitedCount = 0;
    }

    public void OnDespawned()
    {
    }
}
