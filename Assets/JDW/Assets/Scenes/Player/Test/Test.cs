using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void TriggerEnter2D(Collider2D collision)
    {
        Debug.Log("�浹����");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.instance._player.Hp -= 1;
            Debug.Log("�÷��̾� ü�°���");
            if(GameManager.instance._player.Hp == 0)
            {
                Debug.Log("�÷��̾� ���ó��");
            }
        }
    }
}
