using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 startPos;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private GameObject bossFightRespawn;
    
    private void Start() 
    {
        startPos = transform.position;
    }
    
    public IEnumerator Respawn(float duration) 
    {
        Debug.Log($"[PlayerRespawn] Starting respawn process. Duration: {duration} seconds");
        
        // Wait for the specified duration first
        yield return new WaitForSeconds(duration);
        
        Debug.Log($"[PlayerRespawn] Duration completed. Beginning respawn...");
        
        // Determine respawn position
        Vector2 respawnPosition;
        if (questManager && questManager.HasPlacedKey)
        {
            respawnPosition = bossFightRespawn.transform.position;
            Debug.Log($"[PlayerRespawn] Boss fight active - using boss fight respawn position: {respawnPosition}");
        }
        else
        {
            respawnPosition = startPos;
            Debug.Log($"[PlayerRespawn] Normal respawn - using start position: {respawnPosition}");
        }
        
        // Set player position
        transform.position = respawnPosition;
        Debug.Log($"[PlayerRespawn] Player position set to: {transform.position}");
        
        // Always attempt to reset boss when player dies
        Debug.Log("[PlayerRespawn] Attempting to reset boss...");
        GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
        if (bossObj != null)
        {
            Debug.Log($"[PlayerRespawn] Boss object found: {bossObj.name}");
            Boss boss = bossObj.GetComponent<Boss>();
            if (boss != null)
            {
                Debug.Log("[PlayerRespawn] Boss component found. Calling ResetBoss()");
                boss.ResetBoss();
                Debug.Log("[PlayerRespawn] Boss reset completed");
            }
            else
            {
                Debug.LogWarning("[PlayerRespawn] Boss component not found on boss object!");
            }
        }
        else
        {
            Debug.Log("[PlayerRespawn] No boss object found with tag 'Boss' - skipping boss reset");
        }
        
        // Reactivate player components
        Debug.Log("[PlayerRespawn] Reactivating player components...");
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("idleRespawn");
        GetComponent<PlayerHealth>().InitHealth();
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAttack>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
        
        Debug.Log("[PlayerRespawn] Player respawn completed successfully");
    }
}