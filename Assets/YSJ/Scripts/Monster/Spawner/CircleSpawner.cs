using ProjectVS.Interface;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectVS.Monster.Spawner
{
    public class CircleSpawner : Spawner
    {
        public float radius = 5f;

        public override void SpawnUnits(Vector3 spawnPoint, int unitCount)
        {
            Vector3 center = spawnPoint;

            GameObject spanwUnit = GetSpawnUnit();
            if (spanwUnit == null) return;

            for (int i = 0; i < unitCount; i++)
            {
                float angle = (360f / unitCount) * i * Mathf.Deg2Rad;
                Vector3 pos = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                GameObject go = Instantiate(spanwUnit, pos, Quaternion.identity);
                spawned.Add(go);
            }
        }

        public override void InitUnits(GameObject target)
        {

        }
    }
}
