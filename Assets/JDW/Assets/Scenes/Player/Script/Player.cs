using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Hp = 50;
    public float Power = 5;
    public float MoveSpeed = 1;
    public float MagicAttackSpeed = 2;
    public float Defense = 1;
    public float AxAttackSpeed = 3;
    public float SwordAttackSpeed = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = collision.gameObject.layer;

        if(layer == LayerMask.NameToLayer("Monster"))
        {
            Debug.Log("�÷��̾� ü�� ����");
            Hp--;
        }
        if (layer == LayerMask.NameToLayer("HpUp"))
        {
            Debug.Log("�÷��̾� ü�� ����");
            Hp ++;
        }
        if (layer == LayerMask.NameToLayer("SpeedUp"))
        {
            Debug.Log("�÷��̾� �ӵ� ����");
            MoveSpeed ++;
        }
        if (layer == LayerMask.NameToLayer("PowerUp"))
        {
            Debug.Log("�÷��̾� ���ݷ� ����");
            Power ++;
        }
        if (layer == LayerMask.NameToLayer("MagicAttackSpeed"))
        {
            Debug.Log("�÷��̾� ���� ���� �ӵ� ����");
            MagicAttackSpeed -= 0.1f;
        }
        if (layer == LayerMask.NameToLayer("DefenseUp"))
        {
            Debug.Log("�÷��̾� ���� ����");
            Defense++;
        }
        if (layer == LayerMask.NameToLayer("AxAttackSpeed"))
        {
            Debug.Log("�÷��̾� ���� ���� �ӵ� ����");
            AxAttackSpeed -= 0.1f;
        }
        if (layer == LayerMask.NameToLayer("SwordAttackSpeed"))
        {
            Debug.Log("�÷��̾� �� ���� �ӵ� ����");
            SwordAttackSpeed -= 0.1f;
        }
    }
}
