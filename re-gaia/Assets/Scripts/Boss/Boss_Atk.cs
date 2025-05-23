using UnityEngine;
using System.Collections;

public class Boss_Atk : StateMachineBehaviour
{
    private SpriteRenderer bossRenderer;
    private Color originalColor;
    public float hitPauseDuration = 0.15f;
    
    // This MonoBehaviour will be used to run coroutines
    private MonoBehaviour coroutineRunner;
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Get references
        bossRenderer = animator.GetComponent<SpriteRenderer>();
        if (bossRenderer != null)
        {
            originalColor = bossRenderer.color;
        }
        
        // Find a MonoBehaviour to run coroutines
        coroutineRunner = animator.GetComponent<MonoBehaviour>();
        if (coroutineRunner == null)
        {
            // If no specific MonoBehaviour is found, try to get the Boss_Health script
            coroutineRunner = animator.GetComponent<Boss_Health>();
        }
    }
    
    // Called by Boss_Health when the boss is hit during this attack state
    public void OnHitDuringAttack(Animator animator)
    {
        if (coroutineRunner != null)
        {
            Debug.Log("PAUSE HERE WHITE ");
            coroutineRunner.StartCoroutine(HitFlashAndPause(animator));

        }
    }
    
    // Coroutine for white flash and pause effect
    private IEnumerator HitFlashAndPause(Animator animator)
    {
        // Pause animation
        float originalSpeed = animator.speed;
        animator.speed = 0;
        
        if (bossRenderer != null)
        {
            // Flash white
            bossRenderer.color = Color.white;
            
            // Pause for the hit duration
            yield return new WaitForSeconds(hitPauseDuration);
            
            // Return to original color
            bossRenderer.color = originalColor;
        }
        else
        {
            // Just pause if no renderer
            yield return new WaitForSeconds(hitPauseDuration);
        }
        
        // Resume animation
        animator.speed = originalSpeed;
    }
}