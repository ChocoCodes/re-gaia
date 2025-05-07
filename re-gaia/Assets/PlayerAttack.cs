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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (Time.time < nextAttackTime) return;

        movement.SetCanMove(false);

        animator.SetInteger("attackIndex", currentIndex);
        animator.SetTrigger("attack");
        nextAttackTime = Time.time + 1f / attackRate;
        currentIndex = 1 - currentIndex;
    }

    public void EndAttack()
    {
        movement.SetCanMove(true);
    }
}
