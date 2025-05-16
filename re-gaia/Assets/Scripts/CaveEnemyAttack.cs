using UnityEngine;

public class CaveEnemyAttack : MonoBehaviour
{
    [Header("Cave Enemy Attack")]
    public int damage = 15;
    public float knockbackForce = 2f;
    public float knockbackDuration = 0.2f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player)
        {
            player.TakeDamage(damage, knockbackForce, knockbackDuration);
        }
    }
}
