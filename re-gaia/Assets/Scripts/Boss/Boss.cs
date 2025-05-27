// Boss.cs - Fixed IsInSkillRange method and added helper method
using UnityEngine;
public class Boss : MonoBehaviour
{
    [Header("Basic Attack Settings")]
    public float basicAttackCooldown = 2f;
    private float basicLastAttackTime;
    public float basicAttackRange = 7f;
    public int basicAttackdamage = 10;
    public float basicKnockbackForce = 10f;
    public float basicKnockbackDuration = 0.2f;
    public bool basicWaitingForCooldown = false;

    [Header("Skill Attack Settings")]
    public float skillCooldown = 3f;
    private float skillLastAttackTime;
    public float skillTriggerRange = 15f; // Must be greater than basicAttackRange
    public int skillDamage = 25;
    public float skillKnockbackForce = 10f;
    public float skillKnockbackDuration = 0.5f;
    public bool skillWaitingForCooldown = false;

    [Header("General Settings")]
    public Transform player;
    public bool isFlipped = false;

    private Vector3 initialPosition;
    private Quaternion initialRotation;

    void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    public bool IsAttackOnCooldown()
    {
        return Time.time < basicLastAttackTime + basicAttackCooldown;
    }

    public void ResetAttackCooldown()
    {
        basicLastAttackTime = Time.time;
    }

    public bool IsSkillOnCooldown()
    {
        return Time.time < skillLastAttackTime + skillCooldown;
    }

    public void ResetSkillCooldown()
    {
        skillLastAttackTime = Time.time;
    }

    // Fixed: This should return true when player is far enough for skill attack
    public bool IsInSkillRange()
    {
        if (player == null) return false;
        float horizontalDistance = Mathf.Abs(player.position.x - transform.position.x);
        return horizontalDistance >= skillTriggerRange;
    }

    // Helper method to get current distance to player
    public float GetDistanceToPlayer()
    {
        if (player == null) return float.MaxValue;
        return Mathf.Abs(player.position.x - transform.position.x);
    }

    public bool HasIntroPlayed
    {
        get
        {
            return GetComponent<BossIntroController>()?.hasIntroPlayed ?? true;
        }
    }

    public void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 scale = transform.localScale;

        if (transform.position.x > player.position.x && isFlipped)
        {
            scale.x *= -1;
            transform.localScale = scale;
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            scale.x *= -1;
            transform.localScale = scale;
            isFlipped = true;
        }
    }

    public void ResetBoss()
    {
        Debug.Log("[Boss] Starting boss reset...");
        
        // Reset position and rotation
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        
        Debug.Log($"[Boss] Position reset to: {initialPosition}, Rotation reset to: {initialRotation}");
        
        // Reset GameObject state
        gameObject.SetActive(true);
        
        // Reset physics
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.simulated = true;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }
        
        GetComponent<Collider2D>().enabled = true;

        // Reset cooldown timers
        basicLastAttackTime = 0f;
        skillLastAttackTime = 0f;

        // Reset animation triggers
        Animator animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = 1f; // Reset animation speed in case it was paused
            animator.ResetTrigger("Attack");
            animator.ResetTrigger("Die");
            animator.ResetTrigger("Idle");
            animator.ResetTrigger("Run");
            animator.ResetTrigger("IntroFinished");
            animator.ResetTrigger("PlayIntro");
            animator.ResetTrigger("Skill");
            
            // Reset to intro idle state
            animator.Play("Boss_IntroIdle", 0, 0f);
        }

        // Reset boss health
        Boss_Health bossHealth = GetComponent<Boss_Health>();
        if (bossHealth != null)
        {
            bossHealth.enabled = true;
            bossHealth.enemyHealthBar.gameObject.SetActive(true);
            
            typeof(Boss_Health).GetField("currentHealth", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(bossHealth, bossHealth.maxHealth);

            bossHealth.enemyHealthBar.SetHealth(bossHealth.maxHealth);
            Debug.Log("[Boss] Health reset to maximum");
        }

        // Reset intro controller
        BossIntroController introController = GetComponent<BossIntroController>();
        if (introController != null)
        {
            introController.ResetIntro();
            Debug.Log("[Boss] Intro controller reset");
        }
        LookAtPlayer();

        // Explode all existing homing projectiles
        HomingProjectile[] projectiles = FindObjectsByType<HomingProjectile>(FindObjectsSortMode.None);
        foreach (HomingProjectile projectile in projectiles)
        {
            projectile.SendMessage("StartExplosion", SendMessageOptions.DontRequireReceiver);
        }

        Debug.Log("[Boss] Boss reset completed");
    }
}