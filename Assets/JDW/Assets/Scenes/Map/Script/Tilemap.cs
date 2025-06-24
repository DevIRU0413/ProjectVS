using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{
    [System.Serializable]
    public class TileData
    {
        public Transform tileTransform;
        public Vector3 originalPosition;
    }

    public TileData[] tiles; // 인스펙터에서 4개 넣기
    public Transform player;

    public void ResetBattleField()
    {
        // 각 타일맵을 원래 위치로 복귀
        foreach (var tile in tiles)
        {
            tile.tileTransform.position = tile.originalPosition;
        }

        // 플레이어를 월드 기준 중심으로 이동
        player.position = Vector3.zero;
    }
}
