using UnityEngine;
using UnityEngine.Tilemaps;

public class BarrierManager : MonoBehaviour {
    private Tilemap barriers;

    void Awake() {
        barriers = GetComponent<Tilemap>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) // Press 'R' to clear barrier
        {
            DestroyAllTiles();
        }
    }

    public void DestroyAllTiles() {
        if (barriers != null) {
            barriers.ClearAllTiles();
            Debug.Log("All tiles destroyed.");
        }
    }
}