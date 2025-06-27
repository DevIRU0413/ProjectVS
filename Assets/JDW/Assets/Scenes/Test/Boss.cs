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
                player.TakeDamage(50);
            }
        }
    }
}
