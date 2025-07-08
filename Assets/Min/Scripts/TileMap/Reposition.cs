using System.Collections;
using System.Collections.Generic;

using ProjectVS.Player.TestMove;

using UnityEngine;


public class Reposition : MonoBehaviour
{
    [SerializeField] private TestPlayer _player;
    [SerializeField] private LayerMask _areaLayer;
    [SerializeField] private int _groundLength;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((_areaLayer.value & (1 << collision.gameObject.layer)) == 0) return;


        Debug.Log($"[Reposition] 플레이어 나감");

        Vector3 playerPos = _player.transform.position;
        Vector3 myPos = transform.position;

        float dirX = playerPos.x - myPos.x;
        float dirY = playerPos.y - myPos.y;

        float diffX = Mathf.Abs(dirX);
        float diffY = Mathf.Abs(dirY);

        dirX = dirX > 0 ? 1f : -1f;
        dirY = dirY > 0 ? 1f : -1f;

        if (diffX > diffY)
            transform.Translate(Vector3.right * dirX * _groundLength * 2f);
        else
            transform.Translate(Vector3.up * dirY * _groundLength * 2f);
    }
}
