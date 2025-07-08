using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiledManager : MonoBehaviour
{
    public GameObject tilePrefab;
    public int tileCount = 3; // 3x3
    public float tileSize = 10f;
    public Transform player;
    private GameObject[,] tiles;

    void Start()
    {
        tiles = new GameObject[tileCount, tileCount];

        // 초기 배치
        for (int x = 0; x < tileCount; x++)
            for (int y = 0; y < tileCount; y++)
            {
                var tile = Instantiate(tilePrefab, new Vector3(x * tileSize, y * tileSize, 0), Quaternion.identity, transform);
                tiles[x, y] = tile;
            }
    }

    void Update()
    {
        Vector3 playerPos = player.position;

        // 오른쪽 경계 넘음
        if (playerPos.x > tileSize)
            ShiftTilesX(-1);
        // 왼쪽 경계 넘음
        if (playerPos.x < 0)
            ShiftTilesX(1);
        // 위쪽 경계 넘음
        if (playerPos.y > tileSize)
            ShiftTilesY(-1);
        // 아래쪽 경계 넘음
        if (playerPos.y < 0)
            ShiftTilesY(1);
    }

    void ShiftTilesX(int dir)
    {
        if (dir > 0)
        {
            // 왼쪽 column을 오른쪽으로 이동
            for (int y = 0; y < tileCount; y++)
            {
                GameObject leftTile = tiles[0, y];
                leftTile.transform.position += Vector3.right * tileSize * tileCount;
            }
            // 배열도 shift
            for (int x = 0; x < tileCount - 1; x++)
                for (int y = 0; y < tileCount; y++)
                    tiles[x, y] = tiles[x + 1, y];
            for (int y = 0; y < tileCount; y++)
                tiles[tileCount - 1, y] = tiles[0, y];
        }
        else
        {
            // 오른쪽 column을 왼쪽으로 이동 (코드 동일하게 작성)
            // 생략
        }
    }

    void ShiftTilesY(int dir)
    {
        if (dir > 0)
        {
            // 아래 row를 위로 이동
            for (int x = 0; x < tileCount; x++)
            {
                GameObject bottomTile = tiles[x, 0];
                bottomTile.transform.position += Vector3.up * tileSize * tileCount;
            }
            // 배열 shift
            for (int y = 0; y < tileCount - 1; y++)
                for (int x = 0; x < tileCount; x++)
                    tiles[x, y] = tiles[x, y + 1];
            for (int x = 0; x < tileCount; x++)
                tiles[x, tileCount - 1] = tiles[x, 0];
        }
        else
        {
            // 위 row를 아래로 이동 (코드 동일하게 작성)
            // 생략
        }
    }
}
