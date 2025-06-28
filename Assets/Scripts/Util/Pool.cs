using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.Util
{
    public class Pool
    {
        private readonly GameObject prefab;
        private readonly Transform parent;
        private readonly Stack<GameObject> poolStack = new Stack<GameObject>();

        public Pool(GameObject prefab, int initialSize, Transform parent = null)
        {
            this.prefab = prefab;
            this.parent = parent;

            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = GameObject.Instantiate(prefab, parent);
                obj.SetActive(false);
                poolStack.Push(obj);
            }
        }

        public GameObject Get()
        {
            GameObject obj = poolStack.Count > 0
                ? poolStack.Pop()
                : GameObject.Instantiate(prefab, parent);

            obj.SetActive(true);
            foreach (var comp in obj.GetComponents<IPoolable>())
                comp.OnSpawned();

            return obj;
        }

        public void Release(GameObject obj)
        {
            foreach (var comp in obj.GetComponents<IPoolable>())
                comp.OnDespawned();

            obj.SetActive(false);
            poolStack.Push(obj);
        }
    }
}
