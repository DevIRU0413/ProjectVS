using System.Collections.Generic;

using ProjectPV.Core.Stat;

namespace ProjectVS.Core.Equipment
{
    public class EquipmentSystem
    {
        private readonly UnitStats _stats;
        private readonly Dictionary<EquipmentSlot, EquipmentData> _equipped = new();

        public EquipmentSystem(UnitStats stats)
        {
            _stats = stats;
        }

        public bool Equip(EquipmentData data)
        {
            if (data == null) return false;

            if (_equipped.TryGetValue(data.slot, out var current))
            {
                Unequip(data.slot);
            }

            _equipped[data.slot] = data;

            foreach (var mod in data.statModifiers)
            {
                var equippedMod = new StatModifier(data.id, mod.statsType, mod.additive, mod.multiplier);
                _stats.AddModifier(equippedMod);
            }

            return true;
        }

        public void Unequip(EquipmentSlot slot)
        {
            if (!_equipped.TryGetValue(slot, out var data)) return;

            _stats.RemoveModifier(data.id);
            _equipped.Remove(slot);
        }

        public EquipmentData GetEquipped(EquipmentSlot slot)
        {
            _equipped.TryGetValue(slot, out var data);
            return data;
        }

        public void UnequipAll()
        {
            foreach (var kvp in _equipped)
            {
                _stats.RemoveModifier(kvp.Value.id);
            }
            _equipped.Clear();
        }
    }
}
