using UnityEngine;

namespace ProjectVS.Monster
{
    public class MonsterBodyColliderHit : Hitable
    {
        protected override void Init()
        {
            base.Init();

            // 능력치
            var controller = GetComponent<MonsterController>();
            _unitStats = controller.Stats;
            SetOnwer(_unitStats);

            OnEnterHitEnd -= HitTimeCheck;
            OnEnterHitEnd += HitTimeCheck;

            _colliderAction.OnTriggerStayAction -= HitTriggerStay;
            _colliderAction.OnTriggerStayAction += HitTriggerStay;

            _colliderAction.OnCollisionStayAction -= HitCollisionStay;
            _colliderAction.OnCollisionStayAction += HitCollisionStay;
        }

        public virtual void HitTriggerStay(Collider2D coll)
        {
            if (_unitStats == null) return;
            if (_hitTime + _unitStats.AtkSpd > Time.time) return;
            Hit(coll.gameObject);
        }

        public virtual void HitCollisionStay(Collision2D coll)
        {
            if (_unitStats == null) return;
            if (_hitTime + _unitStats.AtkSpd > Time.time) return;
            Hit(coll.gameObject);
        }

        private void HitTimeCheck()
        {
            Debug.Log("Hit");
            _hitTime = Time.time;
        }
    }
}
