using System;
using System.Collections.Generic;

using UnityEngine;

namespace ProjectPV.Core.Stat
{
    [Serializable]
    public class UnitStats
    {
        [SerializeField] private float _currentHp;

        [SerializeField] private Dictionary<UnitStatType, float> _baseStats = new();    // 베이스
        private readonly Dictionary<UnitStatType, float> _additiveCache = new();        // 추가   (합연산)
        private readonly Dictionary<UnitStatType, float> _multiplierCache = new();      // 곱연산 (곱연산)

        private readonly List<StatModifier> _modifiers = new();

        public float CurrentHp
        {
            get => _currentHp;
            set => _currentHp = Mathf.Clamp(value, 0, GetFinalStat(UnitStatType.MaxHp));
        }

        public void SetBaseStat(UnitStatType type, float value)
        {
            _baseStats[type] = value;
        }

        public float GetBaseStat(UnitStatType type)
        {
            return _baseStats.TryGetValue(type, out float value) ? value : 0f;
        }

        public void AddModifier(StatModifier modifier)
        {
            _modifiers.Add(modifier);
            RecalculateCaches();
        }

        public void RemoveModifier(string sourceId)
        {
            _modifiers.RemoveAll(m => m.sourceId == sourceId);
            RecalculateCaches();
        }

        public float GetFinalStat(UnitStatType type)
        {
            float baseValue = GetBaseStat(type);
            float additive = _additiveCache.TryGetValue(type, out var add) ? add : 0f;
            float multiplier = _multiplierCache.TryGetValue(type, out var mul) ? mul : 1f;

            return (baseValue + additive) * multiplier;
        }

        private void RecalculateCaches()
        {
            _additiveCache.Clear();
            _multiplierCache.Clear();

            foreach (var modifier in _modifiers)
            {
                if (!_additiveCache.ContainsKey(modifier.statsType))
                    _additiveCache[modifier.statsType] = 0f;

                if (!_multiplierCache.ContainsKey(modifier.statsType))
                    _multiplierCache[modifier.statsType] = 1f;

                _additiveCache[modifier.statsType] += modifier.additive;
                _multiplierCache[modifier.statsType] *= modifier.multiplier;
            }
        }

        public void ClearAllModifiers()
        {
            _modifiers.Clear();
            RecalculateCaches();
        }
    }
}
