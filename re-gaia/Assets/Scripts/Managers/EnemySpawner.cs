using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject prefab;
    public GameObject patrolA;
    public GameObject patrolB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject SpawnEnemy() {
        // Create a new enemy at spawner position and get the reference to the patrol zone
        GameObject enemy = Instantiate(prefab, transform.position, Quaternion.identity);
        EnemyPatrol patrolRef = enemy.GetComponent<EnemyPatrol>();

        patrolRef.pointA = patrolA;
        patrolRef.pointB = patrolB;
        patrolRef.currentPoint = patrolB.transform;
        patrolRef.rb = enemy.GetComponent<Rigidbody2D>();

        return enemy;
    }
}
