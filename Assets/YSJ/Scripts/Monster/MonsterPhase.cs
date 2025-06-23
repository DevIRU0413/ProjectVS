using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class MonsterPhase
{
    [Range(0.0f, 1.0f)]
    public float MonsterHp;
    [Min(0f)]
    public float PhaseChangeDelay = 0.3f;

    public List<MonsterPhasePart> MonsterPattenList;
}
