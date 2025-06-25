using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.GraphicsBuffer;

namespace ProjectVS
{
    public class PureBoidManager : MonoBehaviour
    {
        public GameObject Target { get; private set; } = null;

        public GameObject unitPrefab;
        public int unitCount = 10;
        public Vector2 spawnRange = new Vector2(5, 5);
        public float spawnDistance = 10.0f;

        public bool _isRandomSpawnDirection;
        public List<float> spawnAngleList = new();

        private PureBoid leaderBoid;
        private List<PureBoid> boids = new();

        private void Awake()
        {
            Target = GameObject.FindGameObjectWithTag("Player");
        }
        private void Start()
        {
            SpawnUnits();
            InitUnits();
        }

        private void SpawnUnits()
        {
            Vector3 spawnPoint = Vector3.zero;
            if (Target != null)
                spawnPoint += Target.transform.position;

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

                GameObject unit = Instantiate(unitPrefab, spawnPos, Quaternion.identity);
                var boid = unit.GetComponent<PureBoid>();
                boids.Add(boid);
            }

            if(boids.Count > 0)
                leaderBoid = boids[0];
        }

        private void InitUnits()
        {
            Vector2 direction = Vector2.zero;

            if (Target != null)
                direction = Target.transform.position - leaderBoid.transform.position;

            foreach (var boid in boids)
                boid.Init(boids, direction);

        }
    }
}
