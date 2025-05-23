using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyMetadata {
        public Enemy enemy;
        public Vector3 startPos;
        public Quaternion startRot = Quaternion.identity;
    }

    public List<EnemyMetadata> enemies = new List<EnemyMetadata>();

    private bool isRespawning = false;
    private float respawnDelay = 3f;

    void Start() {
        foreach (EnemyMetadata data in enemies) {
            if (data.enemy == null) {
                Debug.LogWarning("EnemyRespawnManager: Found null enemy reference in enemies list.");
                continue;
            }
            data.startPos = data.enemy.transform.position;
            data.startRot = data.enemy.transform.rotation;
            data.enemy.SetManager(this);
        }
    }

    public void OnEnemyDeath() {
        if (isRespawning) return;

        bool allDead = true;
        foreach (EnemyMetadata data in enemies) {
            if(data.enemy.isAlive) {
                allDead = false;
                break;
            }
        }

        if (allDead) {
            StartCoroutine(RespawnAll());
        }
    }

    IEnumerator RespawnAll() {
        isRespawning = true;
        yield return new WaitForSeconds(respawnDelay);

        foreach(EnemyMetadata data in enemies) {
            RespawnEnemy(data);
        }

        isRespawning = false;
    }

    
    private void RespawnEnemy(EnemyMetadata data) {
        Enemy enemy = data.enemy;

        enemy.gameObject.SetActive(true);
        enemy.transform.position = data.startPos;
        enemy.transform.rotation = data.startRot;
        enemy.Respawn();
    }

    public void RespawnEnemiesWhenQuestStart() {
        isRespawning = true;
        foreach (EnemyMetadata data in enemies) {
            if(!data.enemy.isAlive) {
                RespawnEnemy(data);
            }
        }
        isRespawning = false;
    }
}