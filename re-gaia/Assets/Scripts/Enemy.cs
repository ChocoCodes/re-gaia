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
        animator.SetBool("isDead", true);
        GetComponent<EnemyPatrol>().enabled = false;

        // Only drop loot if the quest is active and not completed
        if(questManager && questManager.hasQuestStarted && !questManager.hasQuestCompleted) {
            DropLoot();
        }

        CapsuleCollider2D[] colliders = GetComponents<CapsuleCollider2D>();
        foreach (CapsuleCollider2D collider in colliders)
        {
            collider.enabled = false;  
        }

        this.enabled = false;
        Destroy(enemyHealthBar.gameObject);
        Destroy(GetComponent<Rigidbody2D>());
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
}
