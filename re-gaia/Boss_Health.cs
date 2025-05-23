using UnityEngine;
using System.Collections;

public class Boss_Health : MonoBehaviour
{
    public int maxHealth = 100;
    public float pauseOnHitDuration = 2f;
    int currentHealth;
    public HealthBar enemyHealthBar;
    private Boss bossMovement;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private float originalAnimSpeed;
    private bool isFlashing = false;
    private bool isIntroPlaying = false;
    
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
        // Don't take damage during intro
        if (isIntroPlaying) return;

        currentHealth -= damage;
        enemyHealthBar.SetHealth(currentHealth);
        
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        
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

    // Called by Boss_Intro when intro starts
    public void OnIntroStart()
    {
        isIntroPlaying = true;
    }

    // Called by Boss_Intro when intro ends
    public void OnIntroEnd()
    {
        isIntroPlaying = false;
    }
    
    // ... rest of the existing code ...
// ... existing code ...
} 