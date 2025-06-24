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
            Debug.Log("플레이어 체력 감소");
            Hp--;
        }
        if (layer == LayerMask.NameToLayer("HpUp"))
        {
            Debug.Log("플레이어 체력 증가");
            Hp ++;
        }
        if (layer == LayerMask.NameToLayer("SpeedUp"))
        {
            Debug.Log("플레이어 속도 증가");
            MoveSpeed ++;
        }
        if (layer == LayerMask.NameToLayer("PowerUp"))
        {
            Debug.Log("플레이어 공격력 증가");
            Power ++;
        }
        if (layer == LayerMask.NameToLayer("MagicAttackSpeed"))
        {
            Debug.Log("플레이어 마법 공격 속도 증가");
            MagicAttackSpeed -= 0.1f;
        }
        if (layer == LayerMask.NameToLayer("DefenseUp"))
        {
            Debug.Log("플레이어 방어력 증가");
            Defense++;
        }
        if (layer == LayerMask.NameToLayer("AxAttackSpeed"))
        {
            Debug.Log("플레이어 도끼 공격 속도 증가");
            AxAttackSpeed -= 0.1f;
        }
        if (layer == LayerMask.NameToLayer("SwordAttackSpeed"))
        {
            Debug.Log("플레이어 검 공격 속도 증가");
            SwordAttackSpeed -= 0.1f;
        }
    }
}
