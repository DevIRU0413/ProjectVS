using ProjectVS.Monster;

using UnityEngine;

namespace ProjectVS
{
    public class MonsterDespawn : MonoBehaviour
    {
        private MonsterController _controller;
        [SerializeField] private MonsterStateType _state;
        private float _currentDespawnDelay = 0.0f;

        private void OnEnable()
        {
            _controller = GetComponent<MonsterController>();
            if (_controller == null || _state == MonsterStateType.None)
            {
                Destroy(this);
                return;
            }

            _currentDespawnDelay = 0.0f;
        }

        void Update()
        {
            if (_controller == null)
                return;

            if(_controller.CurrentStateType == _state)
            {
                _currentDespawnDelay += Time.deltaTime;
                if (_controller.DespawnDelay < _currentDespawnDelay)
                {
                    ProjectVS.Util.PoolManager.Instance.Despawn(this.gameObject);
                }
            }
        }
    }
}
