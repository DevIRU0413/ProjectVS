using System.Collections.Generic;

using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Monster.Spawner
{
    public class LineSpawner : SpawnerBase
    {
        public bool isReverseLine = false;
        public float offset = 1.5f;
        public float distance = 10.0f;
        public List<Vector2> directionList = new();

        public float spawnLifeCycle = 0;

        public override void SpawnUnits(GameObject target, Vector3 spawnPoint, int unitCount)
        {
            if (directionList.Count <= 0) return;

            GameObject spanwUnit = GetSpawnUnit();
            if (spanwUnit == null) return;

            Vector3 randmonDir = directionList[Random.Range(0, directionList.Count)];
            if (randmonDir == Vector3.zero) return;

            // 정방향
            Vector3 forwardSpawnPoint = spawnPoint + (randmonDir.normalized * distance);
            lineSpawn(spanwUnit, target, spawnPoint, forwardSpawnPoint, unitCount);
            SpawnPositionList.Add(forwardSpawnPoint);

            // 역방향
            if (!isReverseLine) return;
            Vector3 reverseSpawnPoint = spawnPoint + (-randmonDir.normalized * distance);
            lineSpawn(spanwUnit, target, spawnPoint, reverseSpawnPoint, unitCount);
            SpawnPositionList.Add(reverseSpawnPoint);
        }

        // 따로 컴포넌트 달아주거나 하는 설정하는 곳
        public override void InitUnits(GameObject target)
        {

        }

        private void lineSpawn(GameObject spanwUnit, GameObject target, Vector3 originPoint, Vector3 spawnPoint, int spawnCount)
        {
            Vector3 moveDir = (originPoint - spawnPoint).normalized;

            // 오른쪽, 왼쪽 방향 계산 (2D 기준 z축 기준 회전)
            Vector3 rightDir = Quaternion.Euler(0, 0, 90) * moveDir;
            Vector3 leftDir  = Quaternion.Euler(0, 0, -90) * moveDir;

            for (int i = 0; i < spawnCount; i++)
            {
                int offsetIndex = (i + 1) / 2;
                float offsetDistance = offsetIndex * offset;

                Vector3 sideDir = (i % 2 == 0) ? leftDir : rightDir;
                Vector3 spawnPos = spawnPoint + sideDir * offsetDistance;

                // GameObject go = GameObject.Instantiate(spanwUnit, spawnPos, Quaternion.identity);
                // spawned.Add(go);
                GameObject go = ProjectVS.Util.PoolManager.ForceInstance.Spawn(spanwUnit.name, spawnPos, Quaternion.identity);

                // 이동 위임
                var monUnit = go.GetOrAddComponent<MonsterController>();
                monUnit.SetTarget(target);

                monUnit.DelegateMovementAuthority();
                monUnit.SetMoveDirection(moveDir, true);

                var ctrl = go.GetComponent<MonsterController>();
                if (ctrl && spawnLifeCycle > 0)
                {
                    var lifeCmp = go.GetOrAddComponent<SpawnObjectLifeCycle>();
                    lifeCmp.SetLifeTime(spawnLifeCycle);
                }
            }
        }
    }
}
