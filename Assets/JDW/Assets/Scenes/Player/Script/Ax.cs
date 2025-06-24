using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ax : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] public float FirePower;

    private void Start()
    {
        // ���콺 ��ġ�� ����������� ��ȯ
        Vector3 _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseWorldPos.z = 0f;
        // ���� ������Ʈ ��ġ�������콺 ��ġ������ ���� ���
        Vector2 _dir = (_mouseWorldPos - transform.position).normalized;

        transform.up = _dir;
        // �ش� �������� ������ ���� ����
        _rigid.AddForce(_dir * FirePower, ForceMode2D.Impulse);
    }
}
