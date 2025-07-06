using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    // 데미지, 관통력
    public float damage;
    public int penetratingPower;

    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    // 초기화
    public void Init(float damage, int penetratingPower, Vector3 dir)
    {
        this.damage = damage;
        this.penetratingPower = penetratingPower;

        // 관통력이 무한보다(-1) 크다면 속도가 있음
        if (penetratingPower > -1)
        {
            _rigid.velocity = dir * 15f;
        }
    }

    /// <summary>
    /// Enemy tag로 반응하는 함수. 필요시 삭제 또는 tag 변경
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Monster") || penetratingPower == -1) return;

        penetratingPower--;

        if (penetratingPower == -1)
        {
            _rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
