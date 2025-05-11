using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public HealthBar enemyHealthBar;

    [Header("Enemy")]
    public int maxHealth = 100;
    int currentHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        enemyHealthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        enemyHealthBar.SetHealth(currentHealth);

        StartCoroutine(FlashWhite());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        animator.SetBool("isDead", true);
        GetComponent<EnemyPatrol>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(enemyHealthBar.gameObject);
        Destroy(GetComponent<Rigidbody2D>());
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
