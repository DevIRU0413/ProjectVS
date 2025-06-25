using ProjectVS.Manager;

using UnityEngine;

public class Test : MonoBehaviour
{
    private void TriggerEnter2D(Collider2D collision)
    {
        Debug.Log("충돌감지");
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.player.Hp -= 1;
            Debug.Log("플레이어 체력감소");
            if (GameManager.Instance.player.Hp == 0)
            {
                Debug.Log("플레이어 사망처리");
            }
        }
    }
}
