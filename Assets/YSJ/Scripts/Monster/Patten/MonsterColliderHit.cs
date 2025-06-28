using UnityEngine;

namespace ProjectVS.Monster
{
    public class MonsterColliderHit : Hitable
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
        }
        public virtual void HitTriggerStay(Collider2D collider)
        {
            if (_unitStats == null) return;
            if (_hitTime + _unitStats.AtkSpd > Time.time) return;
            Hit(collider);
        }
        private void HitTimeCheck()
        {
            _hitTime = Time.time;
        }
    }
}
