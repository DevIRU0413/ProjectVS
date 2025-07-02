using System.Collections;
using System.Collections.Generic;
using ProjectVS;

using UnityEngine;

[CreateAssetMenu(fileName = "Character Stats Data", menuName = "Scriptable Object/Character Stats Data")]
public class CharacterStatsSO : ScriptableObject
{
    public CharacterClass CharacterClass;

    // base
    private float _baseHp;          // 체력
    private float _baseAtk;         // 공격력
    private float _baseDfs;         // 방어력
    private float _baseSpd;         // 이속
    private float _baseAtkSpd;      // 공속
}
