using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : MonoBehaviour
{
    // ������, �����
    public float damage;
    public int penetratingPower;

    // �Ѿ� ��. ���߿��� �����ߵ�
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    // �ʱ�ȭ
    public void Init(float damage, int penetratingPower, Vector3 dir)
    {
        this.damage = damage;
        this.penetratingPower = penetratingPower;

        // ������� ���Ѻ���(-1) ũ�ٸ� �ӵ��� ����
        if (penetratingPower > -1)
        {
            _rigid.velocity = dir * 15f;
        }
    }

    /// <summary>
    /// Enemy tag�� �����ϴ� �Լ�. �ʿ�� ���� �Ǵ� tag ����
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || penetratingPower == -1) return;

        penetratingPower--;

        if (penetratingPower == -1)
        {
            _rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}
