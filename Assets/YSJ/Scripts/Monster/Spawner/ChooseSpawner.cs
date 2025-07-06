using UnityEngine;

using static UnityEditor.PlayerSettings;

namespace ProjectVS.Monster.Spawner
{
    public class ChooseSpawner : SpawnerBase
    {
        /// <summary>
        /// 선택 생성기(Edit 용)
        /// </summary>
        /// <param name="target">생성 시킬 오브젝트</param>
        /// <param name="point">생성할 위치</param>
        /// <param name="unitCount">생성할 몬스터의 수</param>
        public override void SpawnUnits(GameObject target, Vector3 point, int unitCount = 1)
        {
            for (int i = 0; i < unitCount; i++)
                GameObject.Instantiate(target, point, Quaternion.identity);
        }

        public override void InitUnits(GameObject target)
        {

        }
    }
}

