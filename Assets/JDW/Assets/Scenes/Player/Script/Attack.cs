using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine.InputSystem;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private float _damage;
 
    public void SetDamage(float damage)
    {
        _damage = damage; // 외부에서 가져올 공격력
    }

    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] public float FirePower;

    private void Start() // 마우스 위치로 공격하기위한 함수
    {

        _rigid = GetComponent<Rigidbody2D>();
        // 마우스 위치 → 월드 위치 (New Input System)
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(
            new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(Camera.main.transform.position.z))
        );
        mouseWorldPos.z = 0f;
        // 현재 오브젝트 위치에서마우스 위치까지의 벡터 계산
        Vector2 _dir = (mouseWorldPos - transform.position).normalized;

        transform.up = _dir;
        // 해당 방향으로 설정한 힘을 가함
        _rigid.AddForce(_dir * FirePower, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            Monster monster = other.GetComponent<Monster>();
            if(monster != null)
            {
                monster.TakeDamage(_damage);
            }
        }
    }
}
