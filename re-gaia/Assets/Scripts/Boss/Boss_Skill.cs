using UnityEngine;
using System.Collections;

public class Boss_Skill : StateMachineBehaviour
{
    private SpriteRenderer bossRenderer;
    private Color originalColor;
    public float hitPauseDuration = 0.15f;
    public float skillDuration = 10f;
    private float skillTimer;
    public HomingProjectile ProjectilePrefab;
    private Transform LaunchOffset;
    
    // This MonoBehaviour will be used to run coroutines
    private MonoBehaviour coroutineRunner;
    private Boss boss;
    private Transform player;
    
    // Keep reference to the running coroutine so we can stop it
    private Coroutine projectileCoroutine;
    
    // Flag to track if this skill state is active
    private bool isSkillActive = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        skillTimer = 0f;
        isSkillActive = true;
        
        bossRenderer = animator.GetComponent<SpriteRenderer>();
        boss = animator.GetComponent<Boss>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        
        if (bossRenderer != null)
        {
            originalColor = bossRenderer.color;
        }
        
        coroutineRunner = animator.GetComponent<MonoBehaviour>();
        if (coroutineRunner == null)
        {
            coroutineRunner = animator.GetComponent<Boss>();
        }
        
        // Start the timed projectile launches and keep reference
        if (coroutineRunner != null)
        {
            projectileCoroutine = coroutineRunner.StartCoroutine(LaunchProjectilesTimed());
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!boss.HasIntroPlayed) return;
        
        skillTimer += Time.deltaTime;
        
        // After skill duration, decide next state
        if (skillTimer >= skillDuration)
        {
            DecideNextState(animator);
        }
    }

    private void DecideNextState(Animator animator)
    {
        // Start skill cooldown only after skill duration ends
        boss.ResetSkillCooldown();
        
        if (player == null || boss == null)
        {
            animator.SetTrigger("Idle");
            return;
        }
        
        float horizontalDistance = Mathf.Abs(player.position.x - boss.transform.position.x);
        
        // Check if player is in basic attack range and attack isn't on cooldown
        if (horizontalDistance <= boss.basicAttackRange)
        {
            if (!boss.IsAttackOnCooldown())
            {
                animator.SetTrigger("Attack");
                boss.ResetAttackCooldown();
                boss.basicWaitingForCooldown = false;
            }
            else
            {
                boss.basicWaitingForCooldown = true;
                animator.SetTrigger("Idle");
            }
        }
        // If player is further away, chase them
        else
        {
            animator.SetTrigger("Run");
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Mark skill as no longer active
        isSkillActive = false;
        
        // Stop the projectile coroutine if it's still running
        if (projectileCoroutine != null && coroutineRunner != null)
        {
            coroutineRunner.StopCoroutine(projectileCoroutine);
            projectileCoroutine = null;
        }
        
        // Reset triggers to prevent conflicts
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Idle");
    }

    private IEnumerator LaunchProjectilesTimed()
    {
        Transform launchPoint = GameObject.Find("SkillLaunchOffset")?.transform;
        if (launchPoint == null || ProjectilePrefab == null)
            yield break;

        float[] launchTimes = { 1f, 3f, 5f };
        
        foreach (float t in launchTimes)
        {
            yield return new WaitForSeconds(t);
            
            // Check if skill is still active before launching projectiles
            if (!isSkillActive)
                yield break;
            
            float[] angleOffsets = { -45f, 0f, 45f };
            foreach (float angle in angleOffsets)
            {
                // Double-check before each projectile instantiation
                if (!isSkillActive)
                    yield break;
                    
                Quaternion rotationOffset = Quaternion.Euler(0f, 0f, angle);
                Instantiate(ProjectilePrefab, launchPoint.position, launchPoint.rotation * rotationOffset);
            }
        }
        
        // Clear the coroutine reference when it completes naturally
        projectileCoroutine = null;
    }
}