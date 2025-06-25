using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] public float FirePower;

    private void Start()
    {
        // 마우스 위치를 월드기준으로 전환
        Vector3 _mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        _mouseWorldPos.z = 0f;
        // 현재 오브젝트 위치에서마우스 위치까지의 벡터 계산
        Vector2 _dir = (_mouseWorldPos - transform.position).normalized;

        transform.up = _dir;
        // 해당 방향으로 설정한 힘을 가함
        _rigid.AddForce(_dir * FirePower,ForceMode2D.Impulse);
    }
}
