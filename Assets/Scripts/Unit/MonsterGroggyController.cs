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
            if (pattern.IsFaild != false)
                _patterns[pattern]++;

            if (pattern.GroggyThreshold <= _patterns[pattern])
            {
                // 그로기 진행
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
