// Boss_Idle.cs - Fixed decision logic
using UnityEngine;
public class Boss_Idle : StateMachineBehaviour
{
    public float idleTime = 1f;
    private float idleTimer;
    private Boss boss;
    private Transform player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleTimer = 0f;
        boss = animator.GetComponent<Boss>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!boss.HasIntroPlayed) return;

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleTime)
        {
            float horizontalDistance = boss.GetDistanceToPlayer();

            // Priority 1: Skill attack (if player is far enough AND skill is not on cooldown)
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
            // Priority 3: Chase player (if not in basic range, or if attacks are on cooldown)
            else
            {
                animator.SetTrigger("Run");
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Run");
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Skill");
    }
}