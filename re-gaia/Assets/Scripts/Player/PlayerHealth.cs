using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar playerHealthBar;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public PlayerMovement movement;
    public PlayerAttack attack;
    public Rigidbody2D rb;
    bool isDead = false;
    bool isTakingDamage = false;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitHealth();
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void InitHealth()
    {
        isDead = false;
        currentHealth = maxHealth;
        playerHealthBar.SetMaxHealth(maxHealth);
    }

    public void HealPlayer(int healAmount)
    {
        if (currentHealth + healAmount > maxHealth)
        {
            healAmount = maxHealth - currentHealth;
        }

        currentHealth += healAmount;
        playerHealthBar.SetHealth(currentHealth);
    }

    public void TakeDamage(int damage, float knockbackForce, float knockbackDuration, Vector2 damageSource)
    {
        if (isTakingDamage) return;

        currentHealth -= damage;
        playerHealthBar.SetHealth(currentHealth);

        StartCoroutine(FlashWhite());

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        isTakingDamage = true;
        animator.SetTrigger("takeDamage");
        StartCoroutine(ApplyKnockback(knockbackForce, knockbackDuration, damageSource));
    }

    // Overloaded method for taking damage without knockback - Polluted Water Zone
    public void TakeDamage(int damage) {
        currentHealth -= damage;
        playerHealthBar.SetHealth(currentHealth);

        StartCoroutine(FlashWhite());
        // Check if the player is already dead
        if (currentHealth <= 0) {
            Die();
            return;
        }

        animator.SetTrigger("takeDamage");
    }

    private void Die()
    {
        animator.SetTrigger("isDead");
        isDead = true;
        attack.EndAttack();
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<PlayerInput>().enabled = false;
        rb.simulated = false;
        this.enabled = false;
        
        StartCoroutine(GetComponent<PlayerRespawn>().Respawn(3f));
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private IEnumerator ApplyKnockback(float knockbackForce, float knockbackDuration, Vector2 damageSource)
    {
        if (currentHealth <= 0) yield break;

        movement.isKnockedback = true;
        movement.SetCanMove(false);

        float direction = Mathf.Sign(transform.position.x - damageSource.x);
        Vector2 knockbackVector = new Vector2(direction * knockbackForce, 0);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackVector, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        rb.linearVelocity = Vector2.zero;
        movement.SetCanMove(true);
        movement.isKnockedback = false;
        isTakingDamage = false; // <- reset flag
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
