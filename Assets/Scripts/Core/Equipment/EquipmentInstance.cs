using System.Collections.Generic;
using ProjectPV.Core.Stat;

using ProjectVS.Core.Stat;

namespace ProjectVS.Core.Equipment
{
    public class EquipmentInstance : IStatProvider
    {
        private readonly EquipmentData _data;

        public EquipmentInstance(EquipmentData data)
        {
            _data = data;
        }

        public string Id => $"equipment:{_data.ID}";

        public IEnumerable<StatModifier> GetModifiers()
        {
            return _data.StatModifiers;
        }

        public EquipmentData Data => _data;
    }
}
