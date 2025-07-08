using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapManager : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _tileSize = 40f;
    [SerializeField] private List<TileUnit> _tileUnits;

    private TileUnit[,] _tiles = new TileUnit[3, 3];
    private Vector2Int _currentCenterGrid = new Vector2Int(1, 1); // 논리적 중심
    private Vector2Int _lastPlayerTileGrid = new Vector2Int(1, 1);

    private void Awake()
    {
        BuildInitialGridFromList();
    }

    private void Update()
    {
        TileUnit closestTile = GetClosestTileToPlayer();
        if (closestTile == null) return;

        Vector2Int playerTileGrid = closestTile.GridPosition;

        // 중심이 바뀐 경우만 로직 실행
        if (playerTileGrid != _lastPlayerTileGrid)
        {
            _lastPlayerTileGrid = playerTileGrid;
            RecenterGrid(playerTileGrid);
        }
    }

    private void BuildInitialGridFromList()
    {
        foreach (TileUnit tile in _tileUnits)
        {
            Vector2Int gridPos = tile.GridPosition;
            if (gridPos.x < 0 || gridPos.x >= 3 || gridPos.y < 0 || gridPos.y >= 3)
            {
                Debug.LogError($"[TileMapManager] 잘못된 그리드 좌표: {gridPos}");
                continue;
            }

            _tiles[gridPos.x, gridPos.y] = tile;
        }

        UpdateTilePositions();
    }

    private TileUnit GetClosestTileToPlayer()
    {
        TileUnit closest = null;
        float minDist = float.MaxValue;

        foreach (TileUnit tile in _tileUnits)
        {
            float dist = Vector3.Distance(_player.position, tile.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closest = tile;
            }
        }

        return closest;
    }

    /// <summary>
    /// 플레이어가 있는 타일을 중심으로 타일배열 재정렬
    /// </summary>
    private void RecenterGrid(Vector2Int newCenter)
    {
        int dx = 1 - newCenter.x;
        int dy = 1 - newCenter.y;

        TileUnit[,] newTiles = new TileUnit[3, 3];

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                int srcX = x - dx;
                int srcY = y - dy;

                if (srcX >= 0 && srcX < 3 && srcY >= 0 && srcY < 3)
                {
                    newTiles[x, y] = _tiles[srcX, srcY];
                }
            }
        }

        _tiles = newTiles;

        UpdateTilePositions();
    }

    /// <summary>
    /// 중심을 기준으로 타일 실제 위치 재배치
    /// </summary>
    public void UpdateTilePositions()
    {
        Vector3 centerPos = _player.position;

        for (int y = 0; y < 3; y++)
        {
            for (int x = 0; x < 3; x++)
            {
                TileUnit tile = _tiles[x, y];
                if (tile == null) continue;

                int offsetX = x - 1;
                int offsetY = y - 1;

                Vector3 newPos = centerPos + new Vector3(offsetX * _tileSize, offsetY * _tileSize, 0f);
                tile.SetWorldPosition(newPos);
            }
        }
    }
}
