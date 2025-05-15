using UnityEngine;

public class SanctuaryEnemyAttack : MonoBehaviour
{
    public Animator animator;

    [Header("Sanctuary Enemy Attack")]
    public int damage = 15;
    public float detectionRange = 5f;
    public LayerMask playerLayer;
    private Transform player;
    public EnemyPatrol patrol;
    public bool isAttacking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        patrol.isPaused = PlayerInSight() || isAttacking;

        if (PlayerInSight() && !isAttacking)
        {
            //ShootAtPlayer();
            Debug.Log("Player in range");
            isAttacking = true;
            animator.SetTrigger("attack");
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

    public void EndAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * transform.localScale.x * detectionRange);
    }
}
