using ProjectVS.Util;

using UnityEngine;

namespace ProjectVS.Monster
{
    public class MonsterBodyColliderHit : Hitable
    {
        private MonsterController _controller;

        protected override void Init()
        {
            if (_hitCollider == null)
                _hitCollider = this.GetComponentInChildren<Collider2D>();
            _colliderAction = _hitCollider.gameObject.GetOrAddComponent<Collider2DAction>();

            // 능력치
            _controller = GetComponent<MonsterController>();
            _unitStats = _controller.Stats;
            SetOnwer(_unitStats);

            _controller.OnSpawn -= OnSpawn;
            _controller.OnSpawn += OnSpawn;

            _controller.OnDespawn -= OnDespawn;
            _controller.OnDespawn += OnDespawn;
        }

        public virtual void HitTriggerStay(Collider2D coll)
        {
            if (_unitStats == null) return;
            if (_hitTime + _unitStats.CurrentAtkSpd > Time.time) return;
            Hit(coll.gameObject);
        }

        public virtual void HitCollisionStay(Collision2D coll)
        {
            if (_unitStats == null) return;
            if (_hitTime + _unitStats.CurrentAtkSpd > Time.time) return;
            Hit(coll.gameObject);
        }

        private void HitTimeCheck()
        {
            Debug.Log("Hit");
            _hitTime = Time.time;
        }

        private void OnSpawn()
        {
            if (_controller.IsDeath) return;

            OnEnterHitEnd -= HitTimeCheck;
            OnEnterHitEnd += HitTimeCheck;


            _colliderAction.OnTriggerStayAction -= HitTriggerStay;
            _colliderAction.OnTriggerStayAction += HitTriggerStay;

            _colliderAction.OnCollisionStayAction -= HitCollisionStay;
            _colliderAction.OnCollisionStayAction += HitCollisionStay;


            _colliderAction.OnTriggerEnterAction -= HitTriggerEnter;
            _colliderAction.OnTriggerEnterAction += HitTriggerEnter;

            _colliderAction.OnCollisionEnterAction -= HitCollisionEnter;
            _colliderAction.OnCollisionEnterAction += HitCollisionEnter;
        }

        private void OnDespawn()
        {
            OnEnterHitEnd -= HitTimeCheck;

            _colliderAction.OnTriggerStayAction -= HitTriggerStay;
            _colliderAction.OnCollisionStayAction -= HitCollisionStay;

            _colliderAction.OnTriggerEnterAction -= HitTriggerEnter;
            _colliderAction.OnCollisionEnterAction -= HitCollisionEnter;
        }
    }
}
