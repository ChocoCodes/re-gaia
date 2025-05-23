using JetBrains.Annotations;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Transform target;
    private Rigidbody2D rb;
    public float speed = 5f;
    public float rotateSpeed = 200f;
    Boss boss;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject bossObject = GameObject.FindGameObjectWithTag("Boss");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogWarning("Player not found. Make sure the player GameObject is tagged as 'Player'.");
        }

        if (bossObject != null)
        {
            boss = bossObject.GetComponent<Boss>();
        }
        else
        {
            Debug.LogWarning("Boss not found. Make sure the Boss GameObject is tagged correctly and has a Boss script.");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, boss.isFlipped ? -transform.right : transform.right).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.linearVelocity = (boss.isFlipped ? -transform.right : transform.right) * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HomingProjectile>() != null)
        {
            return;
        }
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                // Calculate knockback direction based on position
                Vector2 hitDirection = collision.transform.position - transform.position;
                hitDirection.Normalize();

                // Apply damage and knockback
                playerHealth.TakeDamage(boss.skillDamage, boss.skillKnockbackForce, boss.skillKnockbackDuration, transform.position);


            }
        }
        // Trigger explosion animation
        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }
    }

    // triggered at an animation event
    public void ExplodeEnd()
    {
        Destroy(gameObject);
    }
}
