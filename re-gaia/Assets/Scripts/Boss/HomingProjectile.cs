using JetBrains.Annotations;
using UnityEngine;
using System.Collections;

public class HomingProjectile : MonoBehaviour
{
    private Animator animator;
    private Transform target;
    private Rigidbody2D rb;
    public float speed = 5f;
    public float rotateSpeed = 200f;
    Boss boss;
    private bool isExploding = false;

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

    void FixedUpdate()
    {
        // Don't move if exploding or if target/boss is null
        if (isExploding || target == null || boss == null)
            return;

        Vector2 direction = (Vector2)target.position - rb.position;
        direction.Normalize();
        float rotateAmount = Vector3.Cross(direction, boss.isFlipped ? transform.right : -transform.right).z;
        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.linearVelocity = (boss.isFlipped ? 1 : -1) * transform.right * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tracker"))
            return;

        // Prevent multiple explosions
        if (isExploding)
            return;

        if (collision.GetComponent<HomingProjectile>() != null)
        {
            return;
        }

        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null && boss != null)
            {
                // Calculate knockback direction based on position
                Vector2 hitDirection = collision.transform.position - transform.position;
                hitDirection.Normalize();
                // Apply damage and knockback
                playerHealth.TakeDamage(boss.skillDamage, boss.skillKnockbackForce, boss.skillKnockbackDuration, transform.position);
            }
        }

        // Start explosion
        StartExplosion();
    }

    private void StartExplosion()
    {
        if (isExploding)
            return;

        isExploding = true;
        
        // Stop movement
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // Trigger explosion animation
        if (animator != null)
        {
            animator.SetTrigger("Explode");
        }
        else
        {
            // If no animator, destroy immediately with delay
            StartCoroutine(DestroyAfterDelay(0.1f));
        }
    }

    // Triggered at an animation event
    public void ExplodeEnd()
    {
        // Use coroutine to delay destruction to next frame
        StartCoroutine(DestroyAfterDelay(0f));
    }

    private IEnumerator DestroyAfterDelay(float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);
        else
            yield return null; // Wait one frame

        // Double-check we're still in play mode and object exists
        if (this != null && gameObject != null && Application.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    // Additional safety check
    void OnDestroy()
    {
        // Clean up any references to prevent null reference exceptions
        target = null;
        boss = null;
    }
}