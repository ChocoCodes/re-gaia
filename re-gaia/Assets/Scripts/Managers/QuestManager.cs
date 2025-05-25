using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private int LOOT_REQUIRED = 3; // Adjust to 20 in real game
    public bool hasQuestStarted;
    public bool hasQuestCompleted;
    private PlayerLootManager plm;
    private EnemyRespawnManager erm;
    private BarrierManager bm;
    private bool isInRange = false;
    public bool HasKey { get; private set; } = false;
    public bool HasPlacedKey { get; private set; } = false;
    // Read-only properties
    public int LOOT_COLLECTED => plm.lootCollected;
    public int LOOT_NEEDED => LOOT_REQUIRED;

    public void SetHasKey(bool value) {
        HasKey = value;
    }
    public void SetHasPlacedKey(bool value) {
        HasPlacedKey = value;
    }

    private void Start() {
        hasQuestStarted = false;
        hasQuestCompleted = false;
        plm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLootManager>();
        erm = FindFirstObjectByType<EnemyRespawnManager>();
        bm = FindFirstObjectByType<BarrierManager>();
    }

    void Update() {
        if (isInRange && Input.GetKeyDown(KeyCode.E) && !hasQuestStarted && !hasQuestCompleted) {
            StartQuest();
        }
        if (hasQuestStarted && !hasQuestCompleted && checkPlayerLootCount()) {
            EndQuest();
        }
        if (hasQuestCompleted && HasPlacedKey) {
            bm.DestroyBossRoomBarriers();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isInRange = false;
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
        bm.DestroyKeyBarriers();
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