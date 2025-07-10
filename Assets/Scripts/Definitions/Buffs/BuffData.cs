using System.Collections.Generic;

using ProjectPV.Core.FSM;
using ProjectPV.Core.Stat;

using UnityEngine;

namespace ProjectPV.Definitions.Buff
{
    [CreateAssetMenu(menuName = "Game/Buff/BuffData")]
    public class BuffData : ScriptableObject
    {
        public string id;
        public string displayName;
        public float duration;          // 0이면 무한 지속
        public bool isStackable = false;// 중첩 가능 여부
        public int maxStack = 1;

        public List<StatModifier> modifiers;
        public List<UnitStatusEffect> statusEffects;
    }
}
