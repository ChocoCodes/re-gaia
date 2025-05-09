using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public PlayerMovement movement;
    public Animator animator;

    [Header("Attack")]
    public float attackRate = 2f;
    float nextAttackTime = 0f;
    private int currentIndex = 0;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;

    public void Attack(InputAction.CallbackContext context)
    {
        if (Time.time < nextAttackTime) return;

        movement.SetCanMove(false);

        if (movement.isGrounded())
        {
            animator.SetInteger("attackIndex", currentIndex);
            animator.SetTrigger("attack");
            currentIndex = 1 - currentIndex;
        }
        else
        {
            animator.SetTrigger("jumpAttack");
            movement.isJumpAttacking = true;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit" + enemy.name);
        }

        nextAttackTime = Time.time + 1f / attackRate;
    }

    public void EndAttack()
    {
        movement.isJumpAttacking = false;
        movement.SetCanMove(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
