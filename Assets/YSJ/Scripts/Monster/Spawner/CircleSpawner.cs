using UnityEngine;

namespace ProjectVS.Monster.Spawner
{
    public class CircleSpawner : SpawnerBase
    {
        public float radius = 5.0f;

        /// <summary>
        /// 타겟을 기준으로 원으로 생성됩니다.
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
                float angle = (360f / unitCount) * i * Mathf.Deg2Rad;
                Vector3 pos = center + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
                // GameObject go = GameObject.Instantiate(spanwUnit, pos, Quaternion.identity);
                // spawned.Add(go);

                GameObject go = ProjectVS.Util.PoolManager.ForceInstance.Spawn(spanwUnit.name, pos, Quaternion.identity);
            }
        }

        public override void InitUnits(GameObject target)
        {

        }
    }
}
