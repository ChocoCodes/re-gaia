using UnityEngine;
using System.Collections;

public class Boss_Health : MonoBehaviour
{
    public int maxHealth = 500;
    public float pauseOnHitDuration = 2f;
    int currentHealth;
    public HealthBar enemyHealthBar;
    private Boss bossMovement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float originalAnimSpeed;
    private bool isFlashing = false;
    
    void Start()
    {
        currentHealth = maxHealth;
        enemyHealthBar.SetMaxHealth(maxHealth);
        bossMovement = GetComponent<Boss>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
        
        if (animator != null)
        {
            originalAnimSpeed = animator.speed;
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        enemyHealthBar.SetHealth(currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        SoundManager.PlaySound(SoundType.BOSS_TAKE_HIT, 0.3f);
        
        
        // Look at player when hit (if not already flashing)
        if (!isFlashing && bossMovement != null && bossMovement.player != null)
        {
            // Get direction to player
            bool shouldFaceRight = transform.position.x < bossMovement.player.position.x;

            // Check if we need to flip
            if (shouldFaceRight && !bossMovement.isFlipped || !shouldFaceRight && bossMovement.isFlipped)
            {
                // Use existing flip method in Boss class
                bossMovement.LookAtPlayer();
            }
        }
        
        // Start the flash and pause effect
        if (!isFlashing)
        {
            StartCoroutine(PauseAndFlash());
        }
    }
    
    private IEnumerator PauseAndFlash()
    {
        isFlashing = true;

        // Stop all boss movement during pause
        if (bossMovement != null && bossMovement.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = false;
            rb.bodyType = RigidbodyType2D.Kinematic; // Prevent physics-based movement during pause
        }

        // Save the current animation state
        if (animator != null)
        {
            // Pause animation
            Debug.Log("Animator Speed Before: " + animator.speed);
            animator.speed = 0;
            Debug.Log("Animator Speed After: " + animator.speed);
            // Ensure boss is immovable if attacked during animation
            if (bossMovement != null && bossMovement.TryGetComponent<Rigidbody2D>(out Rigidbody2D attackRb))
            {
                attackRb.bodyType = RigidbodyType2D.Kinematic;
                attackRb.simulated = false;
            }
        }

        // Flash semi-transparent white
        if (spriteRenderer != null)
        {
            // Create a semi-transparent white color
            Color flashColor = new Color(1f, 1f, 1f, 0.75f); 
            spriteRenderer.color = flashColor;
        }

        // Wait for the hit duration
        yield return new WaitForSeconds(pauseOnHitDuration);

        // Resume animation
        if (animator != null)
        {
            animator.speed = originalAnimSpeed;
        }

        // Return to original color
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

        // Re-enable boss movement after pause
        if (bossMovement != null && bossMovement.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb2))
        {
            rb2.bodyType = RigidbodyType2D.Kinematic;
            rb2.simulated = true;
        }

        isFlashing = false;
    }
    
    private void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        StartCoroutine(DisableScripts());
    }

    IEnumerator DisableScripts() {
        yield return new WaitForSeconds(1.5f);
        enemyHealthBar.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        bossMovement.gameObject.SetActive(false);
    }
}