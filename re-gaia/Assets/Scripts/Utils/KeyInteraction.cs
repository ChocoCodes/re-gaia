using UnityEngine;

public class KeyInteraction : MonoBehaviour
{
    [SerializeField] private Sprite keyPlacedSprite;
    [SerializeField] private QuestManager qm;
    private SpriteRenderer sr;
    private bool isInRange = false;
    public GameObject prompt;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        if (!sr) Debug.LogError("No SpriteRenderer found.");
    }

    void Update() {
        // bool questRequirements = qm && qm.hasQuestCompleted && qm.HasKey;
        if (isInRange && qm.HasKey) {
            if (Input.GetKeyDown(KeyCode.E)) PlaceKey();
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isInRange = true;
            if (!qm.HasPlacedKey) prompt.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            isInRange = false;
            prompt.SetActive(false);
        }
    }

    void PlaceKey() {
        if(sr && keyPlacedSprite) {
            sr.sprite = keyPlacedSprite;
            qm.SetHasKey(false); 
            qm.SetHasPlacedKey(true); // Notify QuestManager to destroy the barrier
        }
    }
}
