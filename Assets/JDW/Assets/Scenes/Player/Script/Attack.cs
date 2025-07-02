using UnityEngine;

namespace ProjectVS.JDW
{
    public class Attack : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigid;
        [SerializeField] public float FirePower;
        private float _damage;
        public void SetDamage(float damage)
        {
            _damage = damage; // 외부에서 가져올 공격력
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Monster") && !other.CompareTag("Boss")) return;
            other.GetComponent<Monster>()?.TakeDamage(_damage);
            other.GetComponent<Boss>()?.TakeDamage(_damage);
        }
    }
}
