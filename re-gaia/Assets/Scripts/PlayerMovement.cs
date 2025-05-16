using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 5f;
    float horizontalMovement;
    bool isFacingRight = true;
    private bool canMove = true;
    public bool isKnockedback = false;
    private float cachedInput = 0f;

    [Header("Jumping")]
    public float jumpPower = 10f;
    public bool isJumpAttacking = false;

    [Header("Dash")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashTime;
    private float dashCooldownTimer;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.05f);
    public LayerMask groundLayer;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;


    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.localScale.x);
        if (isKnockedback)
        {
            Gravity();
            animator.SetFloat("yVelocity", rb.linearVelocity.y);
            animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
            return;
        }

        if (dashCooldownTimer > 0f) dashCooldownTimer -= Time.deltaTime;

        if (isDashing)
        {
            rb.linearVelocity = new Vector2((isFacingRight ? 1 : -1) * dashSpeed, 0f);
            dashTime -= Time.deltaTime;

            if (dashTime <= 0f)
            {
               isDashing = false;
            }

            return;
        }

        if (canMove)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            horizontalMovement = 0f;
        }

        //rb.linearVelocity = new Vector2(horizontalMovement * moveSpeed, rb.linearVelocity.y);
       
        Gravity();
        Flip();

        animator.SetFloat("yVelocity", rb.linearVelocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
    }

    public void Move(InputAction.CallbackContext context)
    {/*
        if (!canMove)
        {
            horizontalMovement = 0f;
            return;
        }

        horizontalMovement = context.ReadValue<Vector2>().x;*/
        cachedInput = context.ReadValue<Vector2>().x;

        if (canMove)
        {
            horizontalMovement = cachedInput;
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!isGrounded())
        {
            return;
        }

        if (context.performed)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
            animator.SetTrigger("jump");
        }/*
        else if (context.canceled)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
            animator.SetTrigger("jump");
        }
        */
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing && dashCooldownTimer <= 0f && canMove)
        {
            isDashing = true;
            dashTime = dashDuration;
            dashCooldownTimer = dashCooldown;
            animator.SetTrigger("dash");
        }
    }

    public bool IsDashing()
    {
       return isDashing;
    }


    private void Gravity()
    {
        if (isJumpAttacking)
        {
           rb.gravityScale = 0f;
           rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
           return;
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }

    public bool isGrounded()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }

        return false;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;

        if (canMove)
        {
             // Restore cached movement input
            horizontalMovement = cachedInput;
        }
    }

    //Visualize Ground Check
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
    }
}
