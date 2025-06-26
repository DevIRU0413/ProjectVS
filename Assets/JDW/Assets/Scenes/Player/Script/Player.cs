using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public CharacterClass selectedClass;
    public PlayerStats stats;

    private void Start()
    {
        stats = PlayerClassData.DefaultStats[selectedClass].Clone();
        Debug.Log($"선택 클래스: {selectedClass}, 체력: {stats.Health}, 공격력: {stats.Attack}");
    }
}
