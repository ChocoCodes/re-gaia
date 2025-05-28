using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRespawn : MonoBehaviour
{
    private Vector2 startPos;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private GameObject bossFightRespawn;
    
    private void Awake() 
    {
        startPos = transform.position;
        bossFightRespawn = GameObject.FindGameObjectWithTag("RespawnBossRoom");
        Debug.Log($"[PlayerRespawn] Awake called. Start position set to: {startPos}, Boss fight respawn point at: {bossFightRespawn?.transform.position}");
        if(bossFightRespawn == null) Debug.LogError($"[PlayerRespawn] Boss fight respawn point is not assigned! Rs Boss Fight {bossFightRespawn}");
    }
    
    void Update() {
        // if (questManager && questManager.HasPlacedKey && questManager.hasQuestCompleted) {
        //    Debug.Log($"{questManager} [PlayerRespawn] Player is in boss fight area. Position updated to: {transform.position}");
        //}
        // Debug.Log($"[PlayerRespawn] Update called. Current position: {transform.position}, Start position: {startPos} Respawn Boss Room: {bossFightRespawn != null}");
    }

    public void SetRespawnPosToBoss() {
        startPos = bossFightRespawn.transform.position;
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
        
        // Reset boss only if player was in the boss room
        if (BossRoomTracker.PlayerInBossRoom)
        {
            Debug.Log("[PlayerRespawn] Player was in boss room. Attempting to reset boss...");
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
        }
        else
        {
            Debug.Log("[PlayerRespawn] Player was not in boss room. Boss reset skipped.");
        }
        
        // Reactivate player components
        Debug.Log("[PlayerRespawn] Reactivating player components...");
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("idleRespawn");
        GetComponent<PlayerHealth>().InitHealth();
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAttack>().enabled = true;
        GetComponent<PlayerInput>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
        
        Debug.Log("[PlayerRespawn] Player respawn completed successfully");
    }
}