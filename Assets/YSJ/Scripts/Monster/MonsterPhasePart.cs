using System;
using UnityEngine;

[Serializable]
public class MonsterPhasePart
{
    public MonsterPatten monsterPatten;

    [Range(0.0f, 1.0f)]
    public float Weight = 1f;
}
