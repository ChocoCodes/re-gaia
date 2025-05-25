using UnityEngine;

public class KeyPickup : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            QuestManager qm = FindFirstObjectByType<QuestManager>();
            if(qm) {
                qm.SetHasKey(true);
                Debug.Log("Key picked up");
            }
            Destroy(gameObject);
        }
    }
}