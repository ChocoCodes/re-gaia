using UnityEngine;

public class BossRoomTracker : MonoBehaviour
{
    public static bool PlayerInBossRoom { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInBossRoom = true;
            Debug.Log("Player is in boss room");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null && playerHealth.IsDead())
            {
                // Player is dead, ignore this trigger exit
                return;
            }

            PlayerInBossRoom = false;
            Debug.Log("Player exited boss room");
        }
    }

    public static void ResetTracker()
    {
        PlayerInBossRoom = false;
        Debug.Log("Boss room tracker manually reset.");
    }
}
