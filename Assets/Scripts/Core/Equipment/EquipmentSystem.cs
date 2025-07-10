using System.Collections.Generic;

namespace ProjectVS.Core.Equipment
{
    public class EquipmentSystem
    {
        private readonly UnitStats _unitStats;
        private readonly Dictionary<EquipmentSlot, EquipmentInstance> _equipped = new();

        public EquipmentSystem(UnitStats stats)
        {
            _unitStats = stats;
        }

        /// <summary>
        /// 장비 장착
        /// </summary>
        public void Equip(EquipmentData data)
        {
            var slot = data.Slot;

            // 기존 장비 제거
            if (_equipped.TryGetValue(slot, out var existing))
                Unequip(slot);

            var instance = new EquipmentInstance(data);
            _equipped[slot] = instance;
            _unitStats.AddProvider(instance);
        }

        /// <summary>
        /// 장비 해제
        /// </summary>
        public void Unequip(EquipmentSlot slot)
        {
            if (!_equipped.TryGetValue(slot, out var instance))
                return;

            _unitStats.RemoveProvider(instance.Id);
            _equipped.Remove(slot);
        }

        /// <summary>
        /// 모든 장비 제거
        /// </summary>
        public void UnequipAll()
        {
            foreach (var kv in _equipped)
                _unitStats.RemoveProvider(kv.Value.Id);

            _equipped.Clear();
        }

        public EquipmentInstance GetEquipped(EquipmentSlot slot)
        {
            return _equipped.TryGetValue(slot, out var item) ? item : null;
        }

        public IReadOnlyDictionary<EquipmentSlot, EquipmentInstance> AllEquipped => _equipped;
    }
}
