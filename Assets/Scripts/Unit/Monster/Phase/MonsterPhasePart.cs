using System;

using ProjectVS.Unit.Monster.Pattern;

using UnityEngine;

namespace ProjectVS.Unit.Monster.Phase
{
    [Serializable]
    public class MonsterPhasePart
    {
        public MonsterPattern monsterPattern;

        [Range(0.0f, 1.0f)]
        public float Weight = 1f;

        [Min(0.0f)]
        public float AamageMultiplier = 1f;
    }
}
