// Boss_Run.cs - Fixed version
using UnityEngine;
public class Boss_Run : StateMachineBehaviour
{
    public float speed = 2.5f;
    Transform player;
    Rigidbody2D rb;
    Boss boss;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        boss = animator.GetComponent<Boss>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!boss.HasIntroPlayed) return;

        boss.LookAtPlayer();
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);

        float horizontalDistance = boss.GetDistanceToPlayer();

        // Priority 1: Skill attack (if player is in skill range AND skill is not on cooldown)
        if (horizontalDistance >= boss.skillTriggerRange && !boss.IsSkillOnCooldown())
        {
            animator.SetTrigger("Skill");
            boss.ResetSkillCooldown();
            boss.skillWaitingForCooldown = false;
        }
        // Priority 2: Basic attack (if player is in basic range AND basic is not on cooldown)
        else if (horizontalDistance <= boss.basicAttackRange && !boss.IsAttackOnCooldown())
        {
            animator.SetTrigger("Attack");
            boss.ResetAttackCooldown();
            boss.basicWaitingForCooldown = false;
        }
        // Priority 3: Idle if in basic range but basic attack is on cooldown
        else if (horizontalDistance <= boss.basicAttackRange && boss.IsAttackOnCooldown())
        {
            boss.basicWaitingForCooldown = true;
            animator.SetTrigger("Idle");
        }
        // Continue running if none of the above conditions are met
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Skill");
        animator.ResetTrigger("Idle");
    }
}