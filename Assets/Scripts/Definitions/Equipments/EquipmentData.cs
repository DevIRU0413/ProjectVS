using System.Collections.Generic;

using ProjectPV.Core.Stat;

using UnityEngine;

namespace ProjectVS.Core.Equipment
{
    [CreateAssetMenu(menuName = "Game/Equipment/EquipmentData")]
    public class EquipmentData : ScriptableObject
    {
        public string id;
        public string displayName;
        public EquipmentSlot slot;
        public List<StatModifier> statModifiers;
    }
}
