using System.Collections.Generic;

using ProjectVS.Interface;
using ProjectVS.Monster;
using ProjectVS.Unit.Monster.Pattern;
using ProjectVS.Unit.Monster.Phase;

using UnityEngine;

namespace ProjectVS.Unit.Monster
{
    [RequireComponent(typeof(MonsterController))]
    [RequireComponent(typeof(MonsterPhaseController))]
    public class MonsterGroggyController : MonoBehaviour
    {
        private MonsterController _core;
        private MonsterPhaseController _phase;

        private Dictionary<IGroggyTrackable, int> _patterns;
        private IGroggyTrackable _currentPattern;

        private void Awake()
        {
            _core = GetComponent<MonsterController>();
            _phase = GetComponent<MonsterPhaseController>();
            _patterns = new();

            // 현재 진행 중인 패턴 받아오는 거임.
            _phase.OnPatternEnter -= OnPatternEnter;
            _phase.OnPatternEnter += OnPatternEnter;

            _phase.OnPatternExit -= OnPatternExit;
            _phase.OnPatternExit += OnPatternExit;
        }

        public void OnPatternEnter(MonsterPattern currentPattern)
        {
            _currentPattern = GetRegisterPattern(currentPattern);
        }

        public void OnPatternExit(MonsterPattern currentPattern)
        {
            var pattern = GetRegisterPattern(currentPattern);
            // 현재랑 끝나는 패턴이 다르다면 오류임
            if (_currentPattern != pattern) return;
            // 실패 했다면, 그로기 카운트 추가
            if (_currentPattern.IsFaild == true) _patterns[pattern]++;
            // 그로기 카운터가 됐다면
            if (_currentPattern.GroggyThreshold <= _patterns[pattern])
            {
                // _core.UnLockChangeState();
                // _core.ChangeState(그로기, true);
            }
        }

        private IGroggyTrackable GetRegisterPattern(MonsterPattern currentPattern)
        {
            var pattern = currentPattern as IGroggyTrackable;
            if (pattern == null
                || _patterns.ContainsKey(pattern)) return pattern;
            _patterns.Add(pattern, 0);
            return pattern;
        }
    }
}
