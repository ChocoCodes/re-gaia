using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    private int LOOT_REQUIRED = 1; // Adjust to 20 in real game
    public bool hasQuestStarted;
    public bool hasQuestCompleted;
    private PlayerLootManager plm;
    private EnemyRespawnManager erm;
    private BarrierManager bm;
    private PlayerSlash playerSlash;
    private bool isInRange = false;
    public bool HasKey { get; private set; } = false;
    public bool HasPlacedKey { get; private set; } = false;
    // Read-only properties
    public int LOOT_COLLECTED => plm.lootCollected;
    public int LOOT_NEEDED => LOOT_REQUIRED;

    public GameObject countUI;
    public GameObject keyUI;
    public GameObject prompt;
    public TMP_Text countText;
    public Image blankKey;
    public Sprite filledKey;

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
        playerSlash = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSlash>();
        erm = FindFirstObjectByType<EnemyRespawnManager>();
        bm = FindFirstObjectByType<BarrierManager>();
    }

    void Update() {
        //Debug.Log(isInRange);
        countText.text = plm.lootCollected.ToString();

        if (HasKey)
        {
            blankKey.sprite = filledKey;
        }

        if (isInRange && Input.GetKeyDown(KeyCode.E) && !hasQuestStarted && !hasQuestCompleted) {
            StartQuest();
        }
        if (hasQuestStarted && !hasQuestCompleted && checkPlayerLootCount()) {
            EndQuest();
            countUI.SetActive(false);
        }
        if (hasQuestCompleted && HasPlacedKey) {
            bm.DestroyBossRoomBarriers();
            keyUI.SetActive(false);
            this.enabled = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isInRange = true;
            if (!hasQuestStarted) prompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isInRange = false;
            prompt.SetActive(false);
        }
    }

    void StartQuest() {
        hasQuestStarted = true;
        Debug.Log($"Quest Started {hasQuestStarted}");
        // Enable Enemy Respawn
        erm?.RespawnEnemiesWhenQuestStart();
        // Show Quest UI and Counter
        countUI.SetActive(true);
        keyUI.SetActive(true);
    }

    void EndQuest() {
        hasQuestCompleted = true;
        countText.color = new Color(0.6f, 1f, 0.6f);
        // Debug.Log($"Quest Completed {hasQuestCompleted}");
        // Disable Enemy Respawn
        // Destroy Barrier in Scene
        bm.DestroyKeyBarriers();
        //this.enabled = false;
        playerSlash.isUnlocked = true;
    }

    bool checkPlayerLootCount() {
        if (plm.lootCollected >= LOOT_REQUIRED) {
            EndQuest();
            return true;
        }
        return false;
    }
}