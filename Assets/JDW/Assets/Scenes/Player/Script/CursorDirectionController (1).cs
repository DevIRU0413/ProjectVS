using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDirectionController : MonoBehaviour
{
    [SerializeField] private Transform player;       // �÷��̾� Transform
    [SerializeField] private float distance = 2f;    // �÷��̾���� �Ÿ�

    private void Update()
    {
        RotateToMousePos();
    }

    private void RotateToMousePos()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // 1. ���� ���� ���
        Vector3 dir = (mouseWorldPos - player.position).normalized;

        // 2. �÷��̾� ���� ��ġ �̵�
        transform.position = player.position + dir * distance;

        // 3. ȸ��: �ﰢ���� ���콺�� �ٶ󺸵���
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // ��������Ʈ�� ���� �ٶ󺸰� �־ ���� -90��
    }
}
