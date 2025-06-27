using System.Collections.Generic;

using ProjectVS.Util;

using Unity.VisualScripting;

using UnityEngine;

namespace ProjectVS.Monster.Spawner
{
    public class PureBoidSpawner : Spawner
    {
        public float moveSpeed = 5f;
        public float neighborRadius = 3f;
        public float separationDistance = 1f;

        [Range(0f, 5f)] public float weightFixed = 1.6f;
        [Range(0f, 5f)] public float weightSeparation = 1.5f;
        [Range(0f, 5f)] public float weightAlignment = 1.0f;
        [Range(0f, 5f)] public float weightCohesion = 1.0f;

        public Vector2 spawnRange = new Vector2(5, 5);
        public float spawnDistance = 10.0f;

        public bool _isRandomSpawnDirection = true;
        public List<float> spawnAngleList = new();

        private PureBoid leaderBoid;
        private List<PureBoid> boids = new();

        public override void SpawnUnits(Vector3 spawnPoint, int unitCount)
        {
            GameObject spanwUnit = GetSpawnUnit();
            if (spanwUnit == null) return;

            // 스폰 방향
            if (_isRandomSpawnDirection)
            {
                float spawnAngle = 0.0f;
                if (spawnAngleList.Count <= 0)
                    spawnAngle = Random.Range(0, 360);
                else
                {
                    int idx = Random.Range(0, spawnAngleList.Count);
                    if (idx >= spawnAngleList.Count)
                        idx = spawnAngleList.Count - 1;

                    spawnAngle = spawnAngleList[idx];
                }

                Vector3 direction = new Vector3(Mathf.Cos(spawnAngle * Mathf.Deg2Rad), Mathf.Sin(spawnAngle * Mathf.Deg2Rad), 0) * spawnDistance;
                spawnPoint += direction;
            }

            for (int i = 0; i < unitCount; i++)
            {
                Vector3 spawnPos = spawnPoint + new Vector3(
                    Random.Range(-spawnRange.x, spawnRange.x),
                    Random.Range(-spawnRange.y, spawnRange.y),
                    0);

                GameObject unit = Instantiate(spanwUnit, spawnPos, Quaternion.identity);
                var boid = unit.GetOrAddComponent<PureBoid>();
                spawned.Add(unit);
                boids.Add(boid);
            }

            if (boids.Count > 0)
                leaderBoid = boids[0];
        }

        public override void InitUnits(GameObject Target)
        {
            Vector2 direction = Vector2.zero;

            if (Target != null)
                direction = Target.transform.position - leaderBoid.transform.position;

            foreach (var boid in boids)
            {
                boid.Init(boids, direction);
                boid.SetValue(neighborRadius, separationDistance);
                boid.SetWeight(weightFixed, weightSeparation, weightAlignment, weightCohesion);
            }

        }
    }
}
