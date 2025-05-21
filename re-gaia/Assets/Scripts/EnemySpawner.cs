using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject prefab;
    public float respawnDelay = 1f;
    public bool canRespawn = false;

    private GameObject currentEnemy;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Spawn();
    }

    void Spawn() {
        if (canRespawn) {
            currentEnemy = Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }

    public void StartRespawn() {
        if (canRespawn) StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay() {
        yield return new WaitForSeconds(respawnDelay);
        if (canRespawn) Spawn();
    }
}
