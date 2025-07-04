using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS
{
    public class Despawn : MonoBehaviour
    {
        protected GameObjectConditionEvaluator _conditions = new();     // 조건 넣어주는 곳
        protected IPoolable _poolableObj;                               // 플링 옵젝
        public bool OnceFirstAction = false;                            // 시작 한번만 실행 할것인지 여부

        protected virtual void Awake()
        {
            _poolableObj = GetComponentInParent<IPoolable>();
        }

        private void OnEnable()
        {
            if (_conditions == null)
                _conditions = new();

            if (_poolableObj == null)
                _poolableObj = GetComponentInParent<IPoolable>();
        }

        private void OnDisable()
        {
            if (OnceFirstAction)
                Destroy(this);
        }

        void Update()
        {
            // 최소 필요 조건
            if (_conditions == null || _poolableObj == null) return;

            // 추가 필요 조건
            if (_conditions.EvaluateAll(this.gameObject))
                PoolManager.Instance.Despawn(this.gameObject);
        }
    }
}
