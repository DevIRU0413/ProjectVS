using System.Collections;

using PVS;

using UnityEngine;

public abstract class MonsterPatten : MonoBehaviour, IStateLifecycle
{
    protected MonsterPhaseController _phaseController;

    [Header("MonsterPatten Information")]
    [field: SerializeField] [field: Min(0.0f)]
    private float _cooldown = 1.0f; // 쿨타임

    [field: SerializeField] [field: Min(0.0f)]
    protected float CastDelay { get; private set; } = 0.0f; // 선딜

    [field: SerializeField] [field: Min(0.0f)]
    protected float RecoveryTime { get; private set; } = 0.0f; // 후딜
    [field: SerializeField]
    public MonsterPattenState PattenState { get; protected set; } = MonsterPattenState.None;

    protected float _currentCooldown;
    public virtual void Init(MonsterPhaseController phaseController)
    {
        _phaseController = phaseController;

        _currentCooldown = _cooldown;
        PattenState = MonsterPattenState.None;
    }

    // 실행 조건(외부에서 판단 가능 또는 독립적으로 판단 가능)
    public virtual bool Condition()
    {
        // 페이즈 컨트롤러 있고,
        // 페이즈 컨트롤러의 상태 락이 걸려 있는지(걸려 있다면 다른 상태 처리 중)
        // 죽지 않았는지
        // 쿨여부
        return _phaseController != null
            && !_phaseController.OnwerController.IsStateLock
            && !_phaseController.OnwerController.IsDeath
            && _currentCooldown <= 0;
    }
    // 실행 행동
    protected abstract IEnumerator IE_PlayAction();

    public void Update()
    {
        if (_phaseController == null) return;
        if (_phaseController.IsPattenControlLock) return;

        if (_currentCooldown > 0)
            _currentCooldown -= Time.deltaTime;
    }

    #region Interface Fuc
    public virtual void Enter()
    {
        _phaseController.OnwerController.LockChangeState();
    }
    public void Action()
    {
        PattenState = MonsterPattenState.Play;
        StartCoroutine(IE_PlayAction());
    }
    public virtual void Exit()
    {
        _phaseController.OnwerController.UnLockChangeState();

        _currentCooldown = _cooldown;
        PattenState = MonsterPattenState.None;
    }
    public virtual void Clear()
    {
        _currentCooldown = _cooldown;
        PattenState = MonsterPattenState.None;
    }
    #endregion
}
