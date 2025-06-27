using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Stuff", menuName = "Stuff/StuffData")]
public class Stuff : ScriptableObject
{
    // ������ ���� �̸�
    public string ItemName;

    // UI ����
    public Sprite Icon;

    // ������
    public GameObject Prefab;
}
