using UnityEngine;

public class KeyPickup : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            QuestManager qm = FindFirstObjectByType<QuestManager>();
            if(qm) {
                SoundManager.PlaySound(SoundType.ITEM_PICKUP, 0.5f);
                qm.SetHasKey(true);
                Debug.Log("Key picked up");
            }
            Destroy(gameObject);
        }
    }
}