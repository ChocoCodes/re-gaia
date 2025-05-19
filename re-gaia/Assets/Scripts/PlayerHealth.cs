using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public HealthBar playerHealthBar;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public PlayerMovement movement;
    public Rigidbody2D rb;

    [Header("Health")]
    public int maxHealth = 100;
    private int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        playerHealthBar.SetMaxHealth(maxHealth);
    }

    public void TakeDamage(int damage, float knockbackForce, float knockbackDuration, Vector2 damageSource)
    {
        currentHealth -= damage;
        playerHealthBar.SetHealth(currentHealth);

        StartCoroutine(FlashWhite());

        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        animator.SetTrigger("takeDamage");
        StartCoroutine(ApplyKnockback(knockbackForce, knockbackDuration, damageSource));
    }

    private void Die()
    {
        animator.SetTrigger("isDead");
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerAttack>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        Destroy(GetComponent<Rigidbody2D>());
        this.enabled = false;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    private IEnumerator ApplyKnockback(float knockbackForce, float knockbackDuration, Vector2 damageSource)
    {
     //Debug.Log($"Applying knockback: Force={knockbackForce}, Duration={knockbackDuration}");
        if (currentHealth <= 0) yield break;
        
        movement.isKnockedback = true;
        movement.SetCanMove(false);

        /*float direction = transform.localScale.x < 0 ?
            Mathf.Sign(transform.localScale.x) :
            -Mathf.Sign(transform.localScale.x);*/

        float direction = Mathf.Sign(transform.position.x - damageSource.x);

        Vector2 knockbackVector = new Vector2(direction * knockbackForce, 0);

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockbackVector, ForceMode2D.Impulse);

        yield return new WaitForSeconds(knockbackDuration);

        rb.linearVelocity = Vector2.zero;
        movement.SetCanMove(true);
        movement.isKnockedback = false;
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
