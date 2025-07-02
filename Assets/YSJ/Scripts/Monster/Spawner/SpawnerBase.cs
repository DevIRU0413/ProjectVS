using System.Collections.Generic;

using ProjectVS.Interface;
using UnityEngine;

namespace ProjectVS.Monster.Spawner
{
    public class SpawnerBase : IUnitSpawner
    {
        [SerializeField] protected List<GameObject> spawnableObjectList = new();
        protected List<GameObject> spawned = new();
        protected List<Vector3> spawnPositionList = new();

        public List<Vector3> SpawnPositionList => spawnPositionList;

        public virtual void SpawnUnits(GameObject target, Vector3 spawnPoint, int unitCount) { }
        public virtual void InitUnits(GameObject target) { }

        public void SetSpawnableObjectList(List<GameObject> list)
        {
            if (list == null) return;
            spawnableObjectList.Clear();
            spawnableObjectList = list;
        }

        protected virtual GameObject GetSpawnUnit()
        {
            if (spawnableObjectList.Count == 0) return null;
            if (spawnableObjectList.Count == 1) return spawnableObjectList[0];

            int randIndex = Random.Range(0, spawnableObjectList.Count);
            if (randIndex >= spawnableObjectList.Count)
                randIndex = spawnableObjectList.Count - 1;

            return spawnableObjectList[randIndex];
        }
    }
}
