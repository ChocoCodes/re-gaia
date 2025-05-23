using UnityEngine;

public class PlayerLootManager : MonoBehaviour
{
    [Header("Loot Stats")]
    public int lootCollected = 0;

    public void CollectLoot() {
        lootCollected++;
        Debug.Log("Loot Collected: " + lootCollected);
    }
}
