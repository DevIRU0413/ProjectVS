using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void TriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌감지");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.instance._player.Hp -= 1;
            Debug.Log("플레이어 체력감소");
            if(GameManager.instance._player.Hp == 0)
            {
                Debug.Log("플레이어 사망처리");
            }
        }
    }
}
