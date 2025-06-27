using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stuff", menuName = "Stuff/StuffData")]
public class Stuff : ScriptableObject
{
    // 구분을 위한 이름
    public string ItemName;

    // UI 연동
    public Sprite Icon;

    // 프리팹
    public GameObject Prefab;
}
