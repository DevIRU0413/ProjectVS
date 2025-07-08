using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//public enum TileType
//{
//    Grass,
//    Dirt,
//    Water,
//    Snow
//}
public class InfiniteTileMapManager : MonoBehaviour
{
    //[SerializeField] private Transform _player;
    //[SerializeField] private float _tileSize = 40f;
    //[SerializeField] private int _poolSizePerType = 5;
    //[SerializeField] private List<TilePrefabEntry> _tilePrefabs;

    //private Dictionary<Vector2Int, TileUnit> _tileMap = new();
    //private Dictionary<TileType, Queue<TileUnit>> _tilePools = new();
    //private Dictionary<TileType, GameObject> _tilePrefabLookup = new();

    //private Vector2Int _currentCenter;

    //private void Start()
    //{
    //    InitTilePools();
    //    Vector2Int startCenter = WorldToGrid(_player.position);
    //    _currentCenter = startCenter;
    //    EnsureNeighborTiles(startCenter);
    //}

    //private void Update()
    //{
    //    Vector2Int newCenter = WorldToGrid(_player.position);

    //    if (newCenter != _currentCenter)
    //    {
    //        _currentCenter = newCenter;
    //        EnsureNeighborTiles(_currentCenter);
    //        CleanupFarTiles(_currentCenter);
    //    }
    //}

    //private void InitTilePools()
    //{
    //    foreach (var entry in _tilePrefabs)
    //    {
    //        _tilePools[entry.Type] = new Queue<TileUnit>();
    //        _tilePrefabLookup[entry.Type] = entry.Prefab;

    //        for (int i = 0; i < _poolSizePerType; i++)
    //        {
    //            GameObject go = Instantiate(entry.Prefab);
    //            TileUnit tile = go.GetComponent<TileUnit>();
    //            go.SetActive(false);
    //            _tilePools[entry.Type].Enqueue(tile);
    //        }
    //    }
    //}

    //private TileUnit GetTile(TileType type)
    //{
    //    if (_tilePools[type].Count > 0)
    //    {
    //        TileUnit tile = _tilePools[type].Dequeue();
    //        tile.gameObject.SetActive(true);
    //        return tile;
    //    }

    //    Debug.LogWarning("[TileMap] 풀 부족! 새 타일 생성: " + type);
    //    GameObject go = Instantiate(_tilePrefabLookup[type]);
    //    return go.GetComponent<TileUnit>();
    //}

    //private void ReturnTile(TileUnit tile)
    //{
    //    tile.gameObject.SetActive(false);
    //    _tilePools[tile.TileType].Enqueue(tile);
    //}

    //private void EnsureNeighborTiles(Vector2Int center)
    //{
    //    for (int y = -1; y <= 1; y++)
    //    {
    //        for (int x = -1; x <= 1; x++)
    //        {
    //            Vector2Int pos = center + new Vector2Int(x, y);
    //            if (_tileMap.ContainsKey(pos)) continue;

    //            TileType type = GetTileTypeForPosition(pos);
    //            TileUnit tile = GetTile(type);
    //            tile.SetGridPosition(pos);
    //            tile.SetWorldPosition(GridToWorld(pos));
    //            tile.SetTileType(type);
    //            _tileMap[pos] = tile;
    //        }
    //    }
    //}

    //private void CleanupFarTiles(Vector2Int center)
    //{
    //    List<Vector2Int> keysToRemove = new();

    //    foreach (var kvp in _tileMap)
    //    {
    //        Vector2Int gridPos = kvp.Key;
    //        TileUnit tile = kvp.Value;

    //        int dx = Mathf.Abs(gridPos.x - center.x);
    //        int dy = Mathf.Abs(gridPos.y - center.y);

    //        if (dx > 1 || dy > 1)
    //        {
    //            keysToRemove.Add(gridPos);
    //            ReturnTile(tile);
    //        }
    //    }

    //    foreach (var key in keysToRemove)
    //    {
    //        _tileMap.Remove(key);
    //    }
    //}

    //private TileType GetTileTypeForPosition(Vector2Int gridPos)
    //{
    //    int hash = Mathf.Abs(gridPos.x * 73856093 ^ gridPos.y * 19349663);
    //    return (TileType)(hash % System.Enum.GetValues(typeof(TileType)).Length);
    //}

    //private Vector2Int WorldToGrid(Vector3 worldPos)
    //{
    //    return new Vector2Int(
    //        Mathf.RoundToInt(worldPos.x / _tileSize),
    //        Mathf.RoundToInt(worldPos.y / _tileSize)
    //    );
    //}

    //private Vector3 GridToWorld(Vector2Int gridPos)
    //{
    //    return new Vector3(gridPos.x * _tileSize, gridPos.y * _tileSize, 0f);
    //}
}
