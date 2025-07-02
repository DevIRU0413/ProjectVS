using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace ProjectVS.JDW
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigid;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float damageDelay = 0f;

        private float _damage;

        private HashSet<Collider2D> _hitTargets = new HashSet<Collider2D>(); // 피격 대상 확인
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Monster") && !other.CompareTag("Boss")) return;

            if (_hitTargets.Contains(other)) return;

            _hitTargets.Add(other);

            StartCoroutine(DelayedDamage(other, damageDelay)); // 오브젝트 생성된 뒤 딜레이를 줌
        }

        private IEnumerator DelayedDamage(Collider2D target, float delay)
        {
            yield return new WaitForSeconds(delay);

            if (target == null) yield break;

            target.GetComponent<Monster>()?.TakeDamage(_damage);
            target.GetComponent<Boss>()?.TakeDamage(_damage);
        }

        public void SetDamage(float damage)
        {
            _damage = damage;
        }
    }
}

