using System;
using System.Collections;
using System.Collections.Generic;

using ProjectVS.Interface;
using ProjectVS.JDW;

using UnityEngine;

public class Test_Projectile : MonoBehaviour
{
    private Rigidbody2D _rigid;
    private Vector2 _dir;
    private float _damage;
    private float _speed;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(Vector2 dir, float dmg, float spd)
    {
        _dir = dir;
        _damage = dmg;
        _speed = spd;

        Destroy(gameObject, 3f); // 3초 후 자동 제거
    }

    private void Update()
    {
        transform.Translate(_dir * _speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<Monster>()?.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}
