using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public HealthBar enemyHealthBar;
    public EnemyPatrol patrol;

    [Header("Enemy")]
    public int maxHealth = 100;
    public float pauseOnHitDuration = 0.3f;
    int currentHealth;

    [Header("Loot")]
    public List<LootItem> lootTable = new List<LootItem>();

    // Reference to the QuestManager - enable/disable loot drop
    public QuestManager questManager;
    // Track enemy state
    public bool isAlive = true;
    private EnemyRespawnManager respawnManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
        enemyHealthBar.SetMaxHealth(maxHealth);
        if(questManager == null) {
            questManager = GameObject.FindGameObjectWithTag("QuestManager").GetComponent<QuestManager>();
        }
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
            return;
        }

        StartCoroutine(PausePatrol(pauseOnHitDuration));
    }

    private void Die()
    {
        if(!isAlive) return;
        isAlive = false;
        // Disable the healthbar only
        enemyHealthBar?.gameObject.SetActive(false);
        animator.SetBool("isDead", true);
        // Hide the enemy after death animation
        StartCoroutine(HideAfterAnim());
        // disable patrol script
        //GetComponent<EnemyPatrol>().enabled = false;
        patrol.isPaused = false;
        patrol.enabled = false;


        CapsuleCollider2D[] colliders = GetComponents<CapsuleCollider2D>();
        foreach (CapsuleCollider2D collider in colliders)
        {
            collider.enabled = false;  
        }

        this.enabled = false;
        // Destroy(enemyHealthBar.gameObject);
        Destroy(GetComponent<Rigidbody2D>());

        // Only drop loot if the quest is active and not completed
        if(questManager && questManager.hasQuestStarted && !questManager.hasQuestCompleted) {
            DropLoot();
            // Notify only the respawn manager if it exists
            respawnManager?.OnEnemyDeath();
        }

    }

    IEnumerator HideAfterAnim() {
        yield return new WaitForSeconds(1.5f);
        gameObject.SetActive(false);
    }

    private IEnumerator PausePatrol(float pauseDuration)
    {
        patrol.isPaused = true;
        yield return new WaitForSeconds(pauseDuration);
        patrol.isPaused = false;
    }

    private IEnumerator FlashWhite()
    {
        spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    private void DropLoot() {
        // Generate loot
        foreach(LootItem item in lootTable) {
            float _dropRate = Random.Range(0f, 100f);
            if(_dropRate <= item.dropRate) {
                InstantiateLoot(item.lootPrefab);
                break;
            }
        }
    }

    private void InstantiateLoot(GameObject loot) {
        if(loot) {
            GameObject droppedLoot = Instantiate(loot, transform.position, Quaternion.identity);
        }
    }

    public void SetManager(EnemyRespawnManager manager) {
        respawnManager = manager;
    }

    // Make this virtual for method overriding in Enemy subclasses
    public void Respawn() {
        // Mark the enemy as alive
        animator.SetBool("isDead", false);
        isAlive = true;
        // Set to max health
        currentHealth = maxHealth;
        // enable patrol script and set trigger to move
        patrol.enabled = true;
        gameObject.SetActive(true);
        animator.SetTrigger("respawnTrigger");

        // Re-enable Patrol Rigidbody2D and link it to the script
        Rigidbody2D newRB = gameObject.GetComponent<Rigidbody2D>();
        if(newRB == null) {
            newRB = gameObject.AddComponent<Rigidbody2D>();
            newRB.freezeRotation = true;
        }

        EnemyPatrol _patrolRef = gameObject.GetComponent<EnemyPatrol>();
        if(_patrolRef != null) {
            _patrolRef.rb = newRB;
            //unpause enemy if it dies while attacking
            _patrolRef.isPaused = false;
        }

        CapsuleCollider2D[] colliders = GetComponents<CapsuleCollider2D>();
        foreach (CapsuleCollider2D collider in colliders) {
            collider.enabled = true;
        }

        // Replenish health bar
        enemyHealthBar?.gameObject.SetActive(true);
        enemyHealthBar?.SetMaxHealth(maxHealth);
        enemyHealthBar?.SetHealth(currentHealth);
        
        // Check if this enemy has a SanctuaryEnemyAttack script
        SanctuaryEnemyAttack sanctuaryAtk = GetComponent<SanctuaryEnemyAttack>();
        if (sanctuaryAtk != null) {
            sanctuaryAtk.animator.SetBool("isDead", false);
            sanctuaryAtk.isDead = false;
            sanctuaryAtk.isAttacking = false;
            sanctuaryAtk.enabled = true;
        }
    }
}
