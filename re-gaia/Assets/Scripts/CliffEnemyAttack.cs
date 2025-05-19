using System.Collections;
using UnityEngine;

public class CliffEnemyAttack : MonoBehaviour
{
    public Animator animator;
    public Rigidbody2D rb;
    public bool isDead;

    [Header("Cliff Enemy Attack")]
    public int damage = 10;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    public LayerMask playerLayer;
    public EnemyPatrol patrol;

    [Header("Chase")]
    public float chaseSpeed = 5f;
    public float sightRange = 5f;
    private bool playerInSight = false;
    public Transform player;
    private float distanceToPlayer, minX, maxX, clampedTargetX;
    public float edgeCooldown = 2f;
    private float exitEdgeTime = 0f;
    private bool isPlayerDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isDead = animator.GetBool("isDead");

        if (isDead)
        {
            this.enabled = false;
        }

        distanceToPlayer = Vector2.Distance(transform.position, player.position);
        playerInSight = distanceToPlayer < sightRange;

        if (playerInSight && Time.time >= exitEdgeTime && !isPlayerDead)
        {
            Attack();
        }
        else
        {
            patrol.isChasing = false;
            animator.SetBool("isAttacking", false);
        }
    }

    private void Attack()
    {
        if (ReachedPatrolEdge())
        {
            rb.linearVelocity = Vector2.zero;
            patrol.isChasing = false;
            animator.SetBool("isAttacking", false);
            exitEdgeTime = Time.time + edgeCooldown;
            return;
        }

        patrol.isChasing = true;
        animator.SetBool("isAttacking", true);

        Vector2 direction = (player.position - transform.position);
        direction.y = 0;

        rb.linearVelocity = direction.normalized * chaseSpeed;

        if ((direction.x > 0 && transform.localScale.x < 0) || (direction.x < 0 && transform.localScale.x > 0))
        {
            Flip();
        }

        if (Time.time >= nextAttackTime)
        {
            Collider2D hitPlayer = Physics2D.OverlapCircle(attackPoint.position, attackRange, playerLayer);
            if (hitPlayer)
            {
                PlayerHealth health = hitPlayer.GetComponent<PlayerHealth>();
                health.TakeDamage(damage, knockbackForce, knockbackDuration, transform.position);
                isPlayerDead = health.GetCurrentHealth() < 1;
            }
            nextAttackTime = Time.time + 1f / attackRate;
        }

    }

    private bool ReachedPatrolEdge()
    {
        minX = Mathf.Min(patrol.pointA.transform.position.x, patrol.pointB.transform.position.x);
        maxX = Mathf.Max(patrol.pointA.transform.position.x, patrol.pointB.transform.position.x);
        return transform.position.x <= minX || transform.position.x >= maxX;
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
