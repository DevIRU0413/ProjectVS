using System;

using ProjectVS.Interface;

using UnityEngine;

namespace ProjectVS
{
    // 데미지 받을 수 있음.
    public class Damageable : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        private IDamageable damageable;

        private void Awake()
        {
            var cmp = target.GetComponentInParent<IDamageable>();
            damageable = cmp;
        }

        public void ApplyDamage(DamageInfo info)
        {
            damageable?.TakeDamage(info);
        }
    }
}
