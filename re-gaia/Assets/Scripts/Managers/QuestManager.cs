using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private int LOOT_REQUIRED = 3; // Adjust to 20 in real game
    public bool hasQuestStarted;
    public bool hasQuestCompleted;
    private PlayerLootManager plm;
    private EnemyRespawnManager erm;
    private BarrierManager bm;

    // Read-only properties
    public int LOOT_COLLECTED => plm.lootCollected;
    public int LOOT_NEEDED => LOOT_REQUIRED;

    private void Start() {
        hasQuestStarted = false;
        hasQuestCompleted = false;
        plm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLootManager>();
        erm = FindFirstObjectByType<EnemyRespawnManager>();
        bm = FindFirstObjectByType<BarrierManager>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.E) && !hasQuestStarted && !hasQuestCompleted) {
            StartQuest();
        }
        if (hasQuestStarted && !hasQuestCompleted && checkPlayerLootCount()) {
            EndQuest();
        }
    }

    void StartQuest() {
        hasQuestStarted = true;
        Debug.Log($"Quest Started {hasQuestStarted}");
        // Enable Enemy Respawn
        erm?.RespawnEnemiesWhenQuestStart();
        // Show Quest UI and Counter
    }

    void EndQuest() {
        hasQuestCompleted = true;
        // Debug.Log($"Quest Completed {hasQuestCompleted}");
        // Disable Enemy Respawn
        // Destroy Barrier in Scene
        bm.DestroyAllTiles();
        this.enabled = false;
    }

    bool checkPlayerLootCount() {
        if (plm.lootCollected >= LOOT_REQUIRED) {
            EndQuest();
            return true;
        }
        return false;
    }
}