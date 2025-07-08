using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileUnit : MonoBehaviour
{
    [SerializeField] private Vector2Int _gridPosition;

    public Vector2Int GridPosition => _gridPosition;

    public void SetGridPosition(Vector2Int gridPos)
    {
        _gridPosition = gridPos;
    }

    public void SetWorldPosition(Vector3 worldPos)
    {
        transform.position = worldPos;
    }
}
