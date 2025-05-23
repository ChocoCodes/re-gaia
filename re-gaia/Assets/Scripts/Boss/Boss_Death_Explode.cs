using UnityEngine;

public class Boss_Death_Explode : StateMachineBehaviour
{
    [Header("Knockback Settings")]
    public float explosionRadius = 5f;
    public float explosionForce = 15f; // Force multiplier for knockback
    public LayerMask affectedLayers = Physics2D.AllLayers; // Layers to affect (default: all)
    
    private bool hasExploded = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasExploded = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasExploded && stateInfo.normalizedTime >= 0.8f)
        {
            ApplyKnockback(animator);
        }
    }
    
    private void ApplyKnockback(Animator animator)
    {
        hasExploded = true;
        Vector3 bossPosition = animator.transform.position;
        
        Debug.Log("Boss knockback effect triggered!");
        
        // Visual debug of explosion radius
        Debug.DrawRay(bossPosition, Vector3.right * explosionRadius, Color.red, 3f);
        Debug.DrawRay(bossPosition, Vector3.left * explosionRadius, Color.red, 3f);
        Debug.DrawRay(bossPosition, Vector3.up * explosionRadius, Color.red, 3f);
        Debug.DrawRay(bossPosition, Vector3.down * explosionRadius, Color.red, 3f);
        
        // Get all colliders within explosion radius that match our layer mask
        Collider2D[] colliders = Physics2D.OverlapCircleAll(bossPosition, explosionRadius, affectedLayers);
        Debug.Log($"Found {colliders.Length} colliders in explosion radius");
        
        foreach (Collider2D hit in colliders)
        {
            // Skip the boss's own collider
            if (hit.transform == animator.transform || hit.isTrigger)
                continue;
                
            Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
            
            // Attempt to get rigidbody from parent if there isn't one on this object
            if (rb == null && hit.transform.parent != null)
                rb = hit.transform.parent.GetComponent<Rigidbody2D>();
                
            if (rb != null)
            {
                // Skip static rigidbodies
                if (rb.bodyType == RigidbodyType2D.Static)
                    continue;
                    
                Debug.Log($"Applying force to: {hit.name} (distance: {Vector2.Distance(rb.position, (Vector2)bossPosition)})");
                
                // Get direction from explosion to object (not the other way around)
                Vector2 direction = ((Vector2)hit.transform.position - (Vector2)bossPosition).normalized;
                
                // Calculate force based on distance (closer = stronger force)
                float distance = Vector2.Distance(rb.position, (Vector2)bossPosition);
                float adjustedForce = Mathf.Lerp(explosionForce, explosionForce * 0.5f, distance / explosionRadius);
                
                // Apply an instantaneous impulse force
                rb.linearVelocity = Vector2.zero; // Reset any existing velocity
                rb.AddForce(direction * adjustedForce, ForceMode2D.Impulse);
                
                // Try direct position modification for more reliable movement
                if (rb.bodyType == RigidbodyType2D.Kinematic)
                {
                    Vector3 targetPosition = hit.transform.position + (Vector3)(direction * (adjustedForce * 0.1f));
                    hit.transform.position = targetPosition;
                }
                
                // Alternative approach - directly set velocity
                // rb.velocity = direction * adjustedForce;
                
                // Visual debug of applied force direction
                Debug.DrawRay(hit.transform.position, direction * adjustedForce * 0.5f, Color.green, 1f);
            }
            else
            {
                Debug.Log($"No Rigidbody2D found on: {hit.name}");
            }
        }
    }
}