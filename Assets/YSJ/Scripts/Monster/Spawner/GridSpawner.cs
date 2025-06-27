using ProjectVS.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectVS.Monster.Spawner
{
    public class GridSpawner : Spawner
    {
        public Vector2 gridSize = new Vector2(3, 3);
        public float spacing = 2f;

        public override void SpawnUnits(Vector3 spawnPoint, int unitCount)
        {
            Vector3 basePos = transform.position;
            int rows = Mathf.CeilToInt(gridSize.y);
            int cols = Mathf.CeilToInt(gridSize.x);
            int spawnedCount = 0;

            GameObject spanwUnit = GetSpawnUnit();
            if (spanwUnit == null) return;

            for (int y = 0; y < rows && spawnedCount < unitCount; y++)
            {
                for (int x = 0; x < cols && spawnedCount < unitCount; x++)
                {
                    Vector3 pos = basePos + new Vector3(x * spacing, y * spacing, 0);
                    GameObject go = Instantiate(spanwUnit, pos, Quaternion.identity);
                    spawned.Add(go);
                    spawnedCount++;
                }
            }
        }

        public override void InitUnits(GameObject target)
        {
            
        }
    }
}
