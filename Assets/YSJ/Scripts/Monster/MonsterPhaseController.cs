using System.Collections;
using System.Collections.Generic;

using PVS.Monster;

using UnityEngine;

public class MonsterPhaseController : MonoBehaviour
{
    [SerializeField] public List<MonsterPhase> _phases = new();

    public bool IsPattenControlLock { get; private set; } = false;
    public MonsterController OnwerController { get; private set; }

    private int _currentPhaseIndex = -1;
    private List<MonsterPhasePart> _activePhaseParts = new();
    private MonsterPatten _activePatten = null;

    private void Start() => Init();
    private void Update()
    {
        if (IsPattenControlLock || _currentPhaseIndex == -1) return;

        if (_activePatten == null)
        {
            _activePatten = SelectWeightedPattern();
            _activePatten?.Enter();
            _activePatten?.Action();
        }
        else if (_activePatten.PattenState == MonsterPattenState.Done)
        {
            _activePatten.Exit();
            _activePatten = null;
        }
    }

    private void Init()
    {
        // 플레이어 컨트롤러 가져옴
        OnwerController = GetComponent<MonsterController>();
        if (OnwerController.CurrentStateType == MonsterStateType.Death) return;

        // 패턴들 초기화
        var pattens = this.GetComponents<MonsterPatten>();
        foreach (var patten in pattens)
            patten.Init(this);

        // 페이즈 전환
        CheckPhaseTransition();

        // 맞았을 때, 페이즈 전환
        OnwerController.OnHit += CheckPhaseTransition;
    }

    // 체력 체크해서 페이즈 전환
    private void CheckPhaseTransition()
    {
        if (OnwerController.IsStateLock) return;

        float hpRatio = OnwerController.Status.CurrentHp / OnwerController.Status.CurrentMaxHp;

        for (int i = 0; i < _phases.Count; i++)
        {
            if (i <= _currentPhaseIndex) continue;

            if (hpRatio <= _phases[i].MonsterHp)
            {
                ApplyPhase(i);
                break;
            }
        }
    }

    // 페이즈 적용
    private void ApplyPhase(int newPhaseIndex)
    {
        IsPattenControlLock = true;

        int oldPhaseIndex = _currentPhaseIndex;
        _currentPhaseIndex = newPhaseIndex;

        Debug.Log($"[MonsterPhase] Phase {_currentPhaseIndex} 활성화됨!");

        OnwerController.LockChangeState();

        var oldPhase = (oldPhaseIndex < 0) ? null : _phases[oldPhaseIndex];
        var currentPhase = _phases[_currentPhaseIndex];

        InitPhasePatternFromWeightedParts(oldPhase, currentPhase);
        StartCoroutine(UnlockStateAfterDelay(currentPhase.PhaseChangeDelay));
    }

    // 페이즈 초기화
    private void InitPhasePatternFromWeightedParts(MonsterPhase old, MonsterPhase current)
    {
        // 이전 페이즈 패턴, 강제 종료
        if (old != null)
        {
            foreach (var patten in old.MonsterPattenList)
            {
                patten?.monsterPatten?.Exit();
                patten?.monsterPatten?.Clear();
            }
        }

        // 현재 페이즈 패턴, 초기화
        foreach (var patten in current.MonsterPattenList)
            patten?.monsterPatten?.Init(this);
    }

    // 가중치로 패턴 실행
    private MonsterPatten SelectWeightedPattern()
    {
        var currentPhase = _phases[_currentPhaseIndex];
        float totalWeight = 0.0f;
        _activePhaseParts.Clear();

        // 실행 가능한 패턴 찾기
        foreach (var part in currentPhase.MonsterPattenList)
        {
            if (part.monsterPatten.Condition())
            {
                _activePhaseParts.Add(part);
                totalWeight += part.Weight;
            }
        }

        // 실행 가능 패턴 없다면 리턴
        if (_activePhaseParts.Count == 0)
            return null;
        else if (_activePhaseParts.Count == 1)
            return _activePhaseParts[0].monsterPatten;

        // 실행 가능한 패턴이 여려개라면, 가중치 랜덤 실행
        float rand = Random.Range(0f, totalWeight);
        float cumulative = 0f;
        foreach (var part in currentPhase.MonsterPattenList)
        {
            if (part.monsterPatten == null) continue;

            cumulative += part.Weight;
            if (rand <= cumulative)
                return part.monsterPatten;
        }

        return null;
    }

    private IEnumerator UnlockStateAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnwerController.UnLockChangeState();
        IsPattenControlLock = false;
        Debug.Log("[MonsterPhase] 상태 잠금 해제됨");
    }
}
