using UnityEngine;
using UnityEngine.Tilemaps;

public class BarrierManager : MonoBehaviour {
    [SerializeField] private Tilemap KeyBarriers;
    [SerializeField] private Tilemap BossRoomBarriers;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Press 'R' to clear barrier
        {
            DestroyAllTiles(KeyBarriers);
        }
        if (Input.GetKeyDown(KeyCode.B)) // Press 'B' to clear boss room barrier
        {
            DestroyAllTiles(BossRoomBarriers);
        }
    }

    public void DestroyAllTiles(Tilemap BarrierRef) {
        if (BarrierRef != null) {
            SoundManager.PlaySound(SoundType.BARRIER_DESTROYED, 0.5f);
            BarrierRef.ClearAllTiles();
            Debug.Log($"{BarrierRef} tiles destroyed.");
        }
    }

    public void DestroyKeyBarriers() => DestroyAllTiles(KeyBarriers);
    public void DestroyBossRoomBarriers() => DestroyAllTiles(BossRoomBarriers);
}