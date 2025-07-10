using System.Collections.Generic;

using ProjectPV.Core.Stat;

using UnityEngine;

namespace ProjectVS.Core.Equipment
{
    [CreateAssetMenu(menuName = "ProjectVS/Equipment/EquipmentData")]
    public class EquipmentData : ScriptableObject
    {
        public string ID;
        public string DisplayName;
        public EquipmentSlot Slot;

        public List<StatModifier> StatModifiers = new();
    }
}
