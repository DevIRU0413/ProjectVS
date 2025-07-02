using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Interface
{
    public interface IUnitSpawner
    {
        List<Vector3> SpawnPositionList { get; }

        void SpawnUnits(GameObject target, Vector3 spawnPoint, int unitCount);
        void InitUnits(GameObject target);
    }
}
