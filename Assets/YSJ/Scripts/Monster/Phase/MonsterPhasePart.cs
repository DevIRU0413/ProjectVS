using System;

using ProjectVS.Monster.Pattern;

using UnityEngine;

namespace ProjectVS.Phase
{
    [Serializable]
    public class MonsterPhasePart
    {
        public MonsterPattern monsterPattern;

        [Range(0.0f, 1.0f)]
        public float Weight = 1f;
    }
}
