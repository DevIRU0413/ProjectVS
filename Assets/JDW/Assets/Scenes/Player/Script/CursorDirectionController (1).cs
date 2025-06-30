using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorDirectionController : MonoBehaviour
{
    [SerializeField] private Transform player;       // 플레이어 Transform
    [SerializeField] private float distance = 2f;    // 플레이어와의 거리

    private void Update()
    {
        RotateToMousePos();
    }

    private void RotateToMousePos()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        // 1. 방향 벡터 계산
        Vector3 dir = (mouseWorldPos - player.position).normalized;

        // 2. 플레이어 기준 위치 이동
        transform.position = player.position + dir * distance;

        // 3. 회전: 삼각형이 마우스를 바라보도록
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f); // 스프라이트가 위를 바라보고 있어서 보정 -90도
    }
}
