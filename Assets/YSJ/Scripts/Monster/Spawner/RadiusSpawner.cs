using UnityEngine;

namespace ProjectVS.Monster.Spawner
{
    public class RadiusSpawner : SpawnerBase
    {
        public float radius = 5.0f;

        /// <summary>
        /// 반지름 최대 거리 생성기
        /// </summary>
        /// <param name="target">생성 기준이 될 오브젝트</param>
        /// <param name="point">반지름을 구하기 위한 위치</param>
        /// <param name="unitCount">생성 유닛 수</param>
        public override void SpawnUnits(GameObject target, Vector3 point, int unitCount)
        {
            Vector3 center = target.transform.position;
            GameObject spanwUnit = GetSpawnUnit();
            if (spanwUnit == null) return;

            for (int i = 0; i < unitCount; i++)
            {
                float angle = Random.Range(0, 360.0f) * Mathf.Deg2Rad; // 라디언 구함
                Vector3 pos = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                GameObject go = ProjectVS.Util.PoolManager.ForceInstance.Spawn(spanwUnit.name, pos, Quaternion.identity);
            }
        }

        public override void InitUnits(GameObject target)
        {
            /*foreach (var unit in spawned)
            {
                var unitAI = unit.GetComponent<MonsterController>();
                if (unitAI != null)
                {
                    // unitAI.SetTarget(target);
                }
            }*/
        }
    }
}
