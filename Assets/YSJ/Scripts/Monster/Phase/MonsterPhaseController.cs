using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProjectVS.Monster;
using ProjectVS.Monster.Pattern;
using PVS;

namespace ProjectVS.Phase
{
    public class MonsterPhaseController : MonoBehaviour
    {
        [SerializeField] public List<MonsterPhase> _phases = new();

        public bool IsPatternControlLock { get; private set; } = false;
        public MonsterController OnwerController { get; private set; }

        private int _currentPhaseIndex = -1;
        private List<MonsterPhasePart> _activePhaseParts = new();
        private MonsterPattern _activePattern = null;

        private void Start()
        {
            Init();
        }

        private void Update()
        {
            if (IsPatternControlLock || _currentPhaseIndex == -1)
                return;

            if (_activePattern == null)
            {
                _activePattern = SelectWeightedPattern();
                _activePattern?.Enter();
                _activePattern?.Action();
            }
            else if (_activePattern.PatternState == MonsterPatternState.Done)
            {
                _activePattern.Exit();
                _activePattern = null;
            }
        }

        private void Init()
        {
            OnwerController = GetComponent<MonsterController>();
            if (OnwerController.CurrentStateType == MonsterStateType.Death)
                return;

            var patterns = GetComponents<MonsterPattern>();
            foreach (var pattern in patterns)
            {
                pattern.Init(this);
            }

            CheckPhaseTransition();

            OnwerController.OnHit += CheckPhaseTransition;
        }

        private void CheckPhaseTransition()
        {
            if (OnwerController.IsStateLock)
                return;

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

        private void ApplyPhase(int newPhaseIndex)
        {
            IsPatternControlLock = true;

            int oldPhaseIndex = _currentPhaseIndex;
            _currentPhaseIndex = newPhaseIndex;

            Debug.Log($"[MonsterPhase] Phase {_currentPhaseIndex} 활성화됨!");

            OnwerController.LockChangeState();

            var oldPhase = oldPhaseIndex < 0 ? null : _phases[oldPhaseIndex];
            var currentPhase = _phases[_currentPhaseIndex];

            InitPhasePatternFromWeightedParts(oldPhase, currentPhase);

            StartCoroutine(UnlockStateAfterDelay(currentPhase.PhaseChangeDelay));
        }

        private void InitPhasePatternFromWeightedParts(MonsterPhase old, MonsterPhase current)
        {
            if (old != null)
            {
                foreach (var pattern in old.MonsterPatternList)
                {
                    pattern?.monsterPattern?.Exit();
                    pattern?.monsterPattern?.Clear();
                }
            }

            foreach (var pattern in current.MonsterPatternList)
            {
                pattern?.monsterPattern?.Init(this);
            }
        }

        private MonsterPattern SelectWeightedPattern()
        {
            var currentPhase = _phases[_currentPhaseIndex];
            float totalWeight = 0.0f;
            _activePhaseParts.Clear();

            foreach (var part in currentPhase.MonsterPatternList)
            {
                if (part.monsterPattern != null && part.monsterPattern.Condition())
                {
                    _activePhaseParts.Add(part);
                    totalWeight += part.Weight;
                }
            }

            if (_activePhaseParts.Count == 0)
                return null;
            else if (_activePhaseParts.Count == 1)
                return _activePhaseParts[0].monsterPattern;

            float rand = Random.Range(0f, totalWeight);
            float cumulative = 0f;
            foreach (var part in _activePhaseParts)
            {
                if (part.monsterPattern == null) continue;

                cumulative += part.Weight;
                if (rand <= cumulative)
                    return part.monsterPattern;
            }

            return null;
        }

        private IEnumerator UnlockStateAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);
            OnwerController.UnLockChangeState();
            IsPatternControlLock = false;
            Debug.Log("[MonsterPhase] 상태 잠해제됨");
        }
    }
}

