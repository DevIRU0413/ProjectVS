using ProjectVS;

using UnityEngine;

public class Boss : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerConfig player = other.GetComponent<PlayerConfig>();
            if (player != null)
            {
                Vector2 direction = (player.transform.position - transform.position).normalized;

                DamageInfo damage = new DamageInfo(player.Stats.CurrentAtk, direction)
                {
                    IsCritical = Random.value < 0.2f,
                    CriticalMultiplier = 1.5f,
                    KnockbackForce = 0,
                    IsPiercing = false
                };

                player.TakeDamage(damage);
            }
        }
    }
}
