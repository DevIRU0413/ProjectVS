using System.Collections.Generic;

using ProjectPV.Core.FSM;
using ProjectPV.Core.Stat;

using UnityEngine;

namespace ProjectPV.Definitions.Buff
{
    [CreateAssetMenu(menuName = "Game/Buff/BuffData")]
    public class BuffData : ScriptableObject
    {
        public string ID;
        public string DisplayName;
        public float Duration;          // 0이면 무한 지속
        public bool IsStackable = false;// 중첩 가능 여부
        public int MaxStack = 1;

        public List<StatModifier> Modifiers;
        public List<UnitStatusEffect> StatusEffects;
    }
}
