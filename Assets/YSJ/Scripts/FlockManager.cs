using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS
{
    public class FlockManager : MonoBehaviour
    {
        [Header("Flock Configuration")]
        public GameObject unitPrefab;
        public int unitCount = 10;
        public Vector2 spawnRange = new Vector2(5, 5);
        public float spawnDistance = 10.0f;

        public bool _isLeaderDirectionFixed = false;
        public bool _isRandomSpawnDirection;
        public List<float> spawnAngleList = new();

        private List<LeaderFlockUnit> units = new();
        private Transform leader;
        private GameObject Target = null;
        private Vector2 TargetDir = Vector2.zero;

        private void Awake()
        {
            LeaderFlockUnit.currentLeaders = 0;
        }
        private void Start()
        {
            SpawnUnits();
            AssignLeader(leader);
        }
        private void Update()
        {
            if (leader == null)
            {
                Transform newLeader = FindNewLeader();
                if (newLeader != null)
                {
                    AssignLeader(newLeader);
                    var unit = newLeader.GetComponent<LeaderFlockUnit>();
                    unit?.InheritDirection(TargetDir);
                    Debug.Log($"New leader elected: {newLeader.name}");
                }
            }
            else
            {
                var unit = leader.GetComponent<LeaderFlockUnit>();
                if (unit != null)
                    TargetDir = unit.GetDirection();
            }
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
                LeaderFlockUnit flockUnit = unit.GetComponent<LeaderFlockUnit>();
                units.Add(flockUnit);
            }

            if (units.Count > 0)
                leader = units[0].transform; // 초기 리더
        }
        private void AssignLeader(Transform newLeader)
        {
            Vector2 dir = Vector2.zero;
            if (Target != null && _isLeaderDirectionFixed)
                dir = Target.transform.position - newLeader.position;
            leader = newLeader;
            TargetDir = dir;
            foreach (var unit in units)
            {
                if (unit != null)
                    unit.Init(leader, units, TargetDir);
            }
        }
        private Transform FindNewLeader()
        {
            if (units.Count == 0) return null;

            Vector2 center = Vector2.zero;
            int validCount = 0;

            foreach (var unit in units)
            {
                if (unit == null) continue;
                center += (Vector2)unit.transform.position;
                validCount++;
            }

            if (validCount == 0) return null;
            center /= validCount;

            LeaderFlockUnit closest = null;
            float minDist = float.MaxValue;

            foreach (var unit in units)
            {
                if (unit == null) continue;
                float dist = Vector2.Distance(center, unit.transform.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = unit;
                }
            }

            return closest?.transform;
        }
    }
}
