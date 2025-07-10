using System.Collections.Generic;

using ProjectPV.Core.Buff;
using ProjectPV.Core.Stat;
using ProjectPV.Definitions.Buff;

namespace Game.Core.Buff
{
    public class BuffSystem
    {
        private readonly UnitStat _unitStat;
        private readonly Dictionary<string, BuffInstance> _activeBuffs = new();

        public BuffSystem(UnitStat stat)
        {
            _unitStat = stat;
        }

        public void AddBuff(BuffData data)
        {
            if (_activeBuffs.TryGetValue(data.id, out var existing))
            {
                // 동일 ID 존재 → 리프레시
                existing.remainingTime = data.duration;
                return;
            }

            var instance = new BuffInstance(data);
            _activeBuffs[data.id] = instance;

            foreach (var mod in data.modifiers)
                _unitStat.AddModifier(new StatModifier(data.id, mod.statType, mod.additive, mod.multiplier));

            // 상태 효과 적용 등 추가 처리 가능
        }

        public void RemoveBuff(string buffId)
        {
            if (!_activeBuffs.ContainsKey(buffId)) return;

            _unitStat.RemoveModifier(buffId);
            _activeBuffs.Remove(buffId);
        }

        public void Tick(float deltaTime)
        {
            List<string> toRemove = null;

            foreach (var pair in _activeBuffs)
            {
                pair.Value.Tick(deltaTime);
                if (pair.Value.IsExpired)
                {
                    toRemove ??= new();
                    toRemove.Add(pair.Key);
                }
            }

            if (toRemove != null)
            {
                foreach (var id in toRemove)
                    RemoveBuff(id);
            }
        }

        public bool HasBuff(string id) => _activeBuffs.ContainsKey(id);

        public void ClearAll()
        {
            foreach (var buff in _activeBuffs.Keys)
                _unitStat.RemoveModifier(buff);

            _activeBuffs.Clear();
        }
    }
}
