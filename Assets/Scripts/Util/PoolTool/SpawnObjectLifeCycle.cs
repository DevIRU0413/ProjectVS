using System.Collections;

using UnityEngine;

namespace ProjectVS.Util
{
    [DisallowMultipleComponent]
    public class SpawnObjectLifeCycle : MonoBehaviour
    {
        [SerializeField, Min(0f)]
        private float _lifeTime = 3f;

        private Coroutine _lifeCoroutine;

        private bool _isPoolableObject = false;

        private void Awake()
        {
            if (PoolManager.Instance == null
                || PoolManager.Instance.HasPool(this.gameObject.name)
                || this.GetComponentInParent<IPoolable>() == null)
            {
                return;
            }

            _isPoolableObject = true;
        }

        private void OnEnable()
        {
            // 재활성화 될 때마다 타이머 리셋
            if (_lifeCoroutine != null)
                StopCoroutine(_lifeCoroutine);

            _lifeCoroutine = StartCoroutine(LifeTimer());
        }

        private void OnDisable()
        {
            Destroy(this);
        }

        private IEnumerator LifeTimer()
        {
            yield return new WaitForSeconds(_lifeTime);

            if(_isPoolableObject)
                PoolManager.Instance.Despawn(gameObject);
            else
                Destroy(gameObject);
        }

        public void SetLifeTime(float time)
        {
            _lifeTime = time;

            // 즉시 적용
            if (isActiveAndEnabled)
            {
                if (_lifeCoroutine != null)
                    StopCoroutine(_lifeCoroutine);

                _lifeCoroutine = StartCoroutine(LifeTimer());
            }
        }
    }
}
