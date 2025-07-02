﻿using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Util
{
    public class PoolManager : SimpleSingleton<PoolManager>
    {
        private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();
        private Dictionary<GameObject, string> objectToKeyMap = new Dictionary<GameObject, string>(); // 추가

        public void CreatePool(string key, GameObject prefab, int initialSize = 10)
        {
            if (pools.ContainsKey(key))
            {
                Debug.LogWarning($"[PoolManager] {key} 키는 이미 존재합니다.");
                return;
            }

            pools[key] = new Pool(prefab, initialSize, this.transform);
        }

        public GameObject Spawn(string key, Vector3 position, Quaternion rotation)
        {
            if (!pools.TryGetValue(key, out Pool pool))
            {
                Debug.LogError($"[PoolManager] {key} 키로 생성된 풀이 없습니다.");
                return null;
            }

            GameObject obj = pool.Get();
            obj.transform.SetPositionAndRotation(position, rotation);

            objectToKeyMap[obj] = key; // 풀 키 등록
            return obj;
        }

        public void Despawn(string key, GameObject obj)
        {
            if (!pools.TryGetValue(key, out Pool pool))
            {
                Debug.LogWarning($"[PoolManager] {key} 키가 존재하지 않아 오브젝트를 제거합니다.");
                Destroy(obj);
                return;
            }

            pool.Release(obj);
            objectToKeyMap.Remove(obj);
        }

        public void Despawn(GameObject obj)
        {
            if (objectToKeyMap.TryGetValue(obj, out string key))
            {
                Despawn(key, obj);
            }
            else
            {
                Debug.LogWarning("[PoolManager] 등록되지 않은 오브젝트입니다. Destroy 처리합니다.");
                Destroy(obj);
            }
        }

        public bool HasPool(string key) => pools.ContainsKey(key);
    }
}
