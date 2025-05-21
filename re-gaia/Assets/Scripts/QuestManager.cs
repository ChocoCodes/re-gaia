using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private int LOOT_REQUIRED = 3; // Adjust to 20 in real game
    public bool hasQuestStarted;
    public bool hasQuestCompleted;
    public PlayerLootManager plm;

    private void Start() {
        hasQuestStarted = false;
        hasQuestCompleted = false;
        plm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLootManager>();
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
        // Debug.Log($"Quest Started {hasQuestStarted}");
        // Enable Enemy Respawn
        // Show Quest UI and Counter
    }

    void EndQuest() {
        hasQuestCompleted = true;
        // Debug.Log($"Quest Completed {hasQuestCompleted}");
        // Disable Enemy Respawn
        // Destroy Barrier in Scene
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