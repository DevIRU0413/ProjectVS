using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Interface
{
    public interface IUnitSpawner
    {
        Transform Transform { get; }
        List<Vector3> SpawnPositionList { get; }

        void SpawnUnits(Vector3 spawnPoint, int unitCount);
        void InitUnits(GameObject target);
    }
}
