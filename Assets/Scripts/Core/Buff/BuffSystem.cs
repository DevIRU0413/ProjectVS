using System.Collections.Generic;

using ProjectPV.Core.Buff;
using ProjectPV.Core.FSM;
using ProjectPV.Core.Stat;
using ProjectPV.Definitions.Buff;

using UnityEngine;

namespace ProjectVS.Core.Buff
{
    public class BuffSystem
    {
        private readonly UnitStats _unitStat;
        private readonly HashSet<UnitStatusEffect> _activeStatusEffects = new();
        private readonly Dictionary<string, BuffInstance> _activeBuffs = new();

        public BuffSystem(UnitStats unitStat)
        {
            _unitStat = unitStat;
        }

        public void AddBuff(BuffData data)
        {
            if (_activeBuffs.TryGetValue(data.id, out var existing))
            {
                existing.RefreshOrStack();
                return;
            }

            var instance = new BuffInstance(data);
            _activeBuffs[data.id] = instance;

            ApplyModifiers(instance);
            ApplyStatusEffects(instance);
        }

        public void RemoveBuff(string buffId)
        {
            if (!_activeBuffs.TryGetValue(buffId, out var instance))
                return;

            RemoveModifiers(instance);
            RemoveStatusEffects(instance);
            _activeBuffs.Remove(buffId);
        }

        public void Tick(float deltaTime)
        {
            List<string> expiredBuffs = null;

            foreach (var pair in _activeBuffs)
            {
                var instance = pair.Value;
                instance.Tick(deltaTime);

                if (instance.IsExpired)
                {
                    expiredBuffs ??= new List<string>();
                    expiredBuffs.Add(pair.Key);
                }
            }

            if (expiredBuffs != null)
            {
                foreach (var buffId in expiredBuffs)
                    RemoveBuff(buffId);
            }
        }

        public bool HasBuff(string id) => _activeBuffs.ContainsKey(id);

        public bool HasStatus(UnitStatusEffect status) => _activeStatusEffects.Contains(status);

        public void ClearAll()
        {
            foreach (var instance in _activeBuffs.Values)
            {
                RemoveModifiers(instance);
                RemoveStatusEffects(instance);
            }

            _activeBuffs.Clear();
            _activeStatusEffects.Clear();
        }

        private void ApplyModifiers(BuffInstance instance)
        {
            foreach (var mod in instance.Data.modifiers)
            {
                var scaled = new StatModifier(
                    instance.Data.id,
                    mod.statsType,
                    mod.additive * instance.Stack,
                    Mathf.Pow(mod.multiplier, instance.Stack)
                );
                _unitStat.AddModifier(scaled);
            }
        }

        private void RemoveModifiers(BuffInstance instance)
        {
            _unitStat.RemoveModifier(instance.Data.id);
        }

        private void ApplyStatusEffects(BuffInstance instance)
        {
            foreach (var effect in instance.Data.statusEffects)
                _activeStatusEffects.Add(effect);
        }

        private void RemoveStatusEffects(BuffInstance instance)
        {
            foreach (var effect in instance.Data.statusEffects)
                _activeStatusEffects.Remove(effect);
        }

        public IEnumerable<BuffInstance> GetAllBuffs() => _activeBuffs.Values;
    }
}
