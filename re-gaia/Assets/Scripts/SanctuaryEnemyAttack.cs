using UnityEngine;

public class SanctuaryEnemyAttack : MonoBehaviour
{
    [Header("Sanctuary Enemy Attack")]
    public int damage = 15;
    public float detectionRange = 5f;
    public LayerMask playerLayer;
    private Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInSight())
        {
            //ShootAtPlayer();
            Debug.Log("Player in range");
        }
    }

    private bool PlayerInSight()
    {
        Vector2 direction = transform.right * Mathf.Sign(transform.localScale.x);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, playerLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            player = hit.transform;
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * transform.localScale.x * detectionRange);
    }
}
