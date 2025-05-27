using UnityEngine;

public class MobDrop : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            PlayerLootManager plm = collision.GetComponent<PlayerLootManager>();
            if(plm != null) {
                Debug.Log("Loot collected by player");
                SoundManager.PlaySound(SoundType.ITEM_PICKUP, 0.5f);
                plm.CollectLoot();
                Destroy(gameObject);
            }
        }
    }
}
