using UnityEngine;

public class Boss_Intro : StateMachineBehaviour
{
    public float introTriggerDistance = 20f; // Distance at which intro triggers
    private Transform player;
    private Boss boss;
    private HealthBar healthBar;
    private bool hasPlayedIntro = false;
    private bool isIntroPlaying = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        boss = animator.GetComponent<Boss>();
        healthBar = animator.GetComponent<Boss_Health>().enemyHealthBar;
        
        // Hide health bar initially
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(player.position, animator.transform.position);

        // If player is within trigger distance and intro hasn't played yet
        if (distanceToPlayer <= introTriggerDistance && !hasPlayedIntro && !isIntroPlaying)
        {
            isIntroPlaying = true;
            // Trigger the intro animation
            animator.SetTrigger("PlayIntro");
        }

        // If intro has finished playing
        if (hasPlayedIntro && !isIntroPlaying)
        {
            // Show health bar after intro
            if (healthBar != null)
            {
                healthBar.gameObject.SetActive(true);
            }

            // Transition to idle state
            animator.SetTrigger("Idle");
        }
    }

    // Called when intro animation starts
    public void OnIntroStart()
    {
        isIntroPlaying = true;
    }

    // Called when intro animation ends
    public void OnIntroEnd()
    {
        isIntroPlaying = false;
        hasPlayedIntro = true;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("PlayIntro");
        animator.ResetTrigger("Idle");
    }
} 