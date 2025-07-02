using UnityEngine;

namespace ProjectVS.Monster.Spawner
{
    public class GridSpawner : SpawnerBase
    {
        public Vector2 GridSize = new Vector2(3, 3);
        public float Spacing = 2f;

        public override void SpawnUnits(GameObject target, Vector3 spawnPoint, int unitCount)
        {
            Vector3 basePos = spawnPoint;
            int rows = Mathf.CeilToInt(GridSize.y);
            int cols = Mathf.CeilToInt(GridSize.x);
            int spawnedCount = 0;

            GameObject spanwUnit = GetSpawnUnit();
            if (spanwUnit == null) return;

            for (int y = 0; y < rows && spawnedCount < unitCount; y++)
            {
                for (int x = 0; x < cols && spawnedCount < unitCount; x++)
                {
                    Vector3 pos = basePos + new Vector3(x * Spacing, y * Spacing, 0);
                    // GameObject go = GameObject.Instantiate(spanwUnit, pos, Quaternion.identity);
                    // spawned.Add(go);

                    GameObject go = Util.PoolManager.ForceInstance.Spawn(spanwUnit.name, pos, Quaternion.identity);
                    spawnedCount++;
                }
            }
        }

        public override void InitUnits(GameObject target)
        {

        }
    }
}
