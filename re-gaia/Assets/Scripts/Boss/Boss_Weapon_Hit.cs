using UnityEngine;

public class Boss_Weapon_Hit : MonoBehaviour
{
    Boss boss;
    void Awake()
    {
        boss = GetComponentInParent<Boss>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Calculate knockback direction based on position
                Vector2 hitDirection = collision.transform.position - transform.position;
                hitDirection.Normalize();

                // Apply damage and knockback
                playerHealth.TakeDamage(boss.basicAttackdamage, boss.basicKnockbackForce, boss.basicKnockbackDuration, transform.position);
            }
        }
    }
}