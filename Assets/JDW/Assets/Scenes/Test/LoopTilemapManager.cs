using System.Collections;
using System.Collections.Generic;

using ProjectVS.JDW;

using UnityEngine;

public class LoopTilemapManager : MonoBehaviour
{
    [Header("필수 설정")]
    public Transform player;       // 플레이어 Transform (혹은 카메라)
    public GameObject[] tilePrefabs;  // Tilemap 프리팹 (Pivot: Center)

    [Header("그리드 설정")]
    public int gridSize = 3;       // 홀수만 사용 (3×3, 5×5 등)
    public float tileSize = 40f;   // 타일 하나의 월드 단위 크기

    private GameObject[,] tiles;   // 실시간 배치될 3×3 타일 배열
    void Start()
    {
        // 배열 생성 및 인스턴스화 (위치는 나중에 Update 에서 세팅)
        tiles = new GameObject[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                // 순서대로 배치(패턴, 랜덤, 고정 다 가능)
                var prefab = tilePrefabs[(x + y * gridSize) % tilePrefabs.Length];
                var tile = Instantiate(prefab, GetTilePosition(x, y), Quaternion.identity, transform);
                tiles[x, y] = tile;
            }
        }
    }
    public void SetPlayer(Transform newPlayer)
    {
        this.player = newPlayer;
    }
    Vector3 GetTilePosition(int x, int y)
    {
        return new Vector3(
            (x - gridSize / 2) * tileSize,
            (y - gridSize / 2) * tileSize,
            0
        );
    }
    void Update()
    {
        if (player == null)
            return;
        // 타일맵 관련 코드
        int cx = Mathf.FloorToInt(player.position.x / tileSize);
        int cy = Mathf.FloorToInt(player.position.y / tileSize);

        // 1) 플레이어 기준으로 “격자 좌표” 계산 (정수로 내림)
        int centerX = Mathf.FloorToInt(player.position.x / tileSize);
        int centerY = Mathf.FloorToInt(player.position.y / tileSize);

        // 2) 3×3 배열을 모두 재배치 (각 타일의 월드 포지션을 스냅)
        int half = gridSize / 2;
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                float worldX = (centerX + x - half) * tileSize;
                float worldY = (centerY + y - half) * tileSize;
                tiles[x, y].transform.position = new Vector3(worldX, worldY, 0);
            }
        }
    }
}
