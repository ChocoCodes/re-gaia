using UnityEngine;
using System.Collections;

public class BossIntroController : MonoBehaviour
{
    [Header("Intro Settings")]
    public float triggerDistance = 5f; // Distance at which intro starts
    public float introAnimationDuration = 3f; // Fallback duration if animation events fail
    
    [Header("References")]
    public Animator animator;
    public HealthBar healthBar;
    public Transform player;
    public GameObject bossHealthHUD; // Reference to the Canvas GameObject
    
    [Header("Debug")]
    public bool hasIntroPlayed = false;
    private bool isIntroPlaying = false;
    
    void Start()
    {
        InitializeReferences();
        
        // Hide the HUD at start
        if (bossHealthHUD != null)
        {
            bossHealthHUD.SetActive(false);
        }
        else
        {
            Debug.LogError("Canvas/HUD reference is missing and could not be found automatically!");
        }
    }
    
    private void InitializeReferences()
    {
        // Find Canvas if not assigned
        if (bossHealthHUD == null)
        {
            // Look for the Canvas in children
            bossHealthHUD = transform.Find("Canvas")?.gameObject;
            if (bossHealthHUD == null)
            {
                // Try to find Canvas in parent or siblings
                Canvas canvas = GetComponentInChildren<Canvas>();
                if (canvas != null)
                    bossHealthHUD = canvas.gameObject;
            }
            
            if (bossHealthHUD == null)
                Debug.LogError("Canvas GameObject could not be found as a child or component!");
        }

        // Get Animator if not assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            if (animator == null)
                Debug.LogError("Animator component not found on this GameObject!");
        }

        // Find Player if not assigned
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player GameObject not found! Make sure it's tagged as 'Player'");
            }
        }

        // Find HealthBar if not assigned
        if (healthBar == null && bossHealthHUD != null)
        {
            healthBar = bossHealthHUD.GetComponentInChildren<HealthBar>();
            if (healthBar == null)
                Debug.LogError("HealthBar component not found in Canvas children!");
        }
    }
    
    void Update()
    {
        if (!hasIntroPlayed && !isIntroPlaying && player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            
            if (distance <= triggerDistance)
            {
                StartIntroSequence();
            }
        }
    }
    
    void StartIntroSequence()
    {
        Debug.Log("Starting boss intro sequence");
        isIntroPlaying = true;
        
        if (animator != null)
        {
            animator.SetTrigger("PlayIntro");
            // Start backup coroutine in case animation events don't work
            StartCoroutine(WaitForIntroAnimation());
        }
        else
        {
            Debug.LogError("Cannot play intro - Animator is null");
            OnIntroAnimationFinished(); // Fallback to ensure game doesn't get stuck
        }
    }
    
    // Coroutine as a backup method to end the intro if animation events fail
    private IEnumerator WaitForIntroAnimation()
    {
        // Wait for animation to complete
        yield return new WaitForSeconds(introAnimationDuration);
        
        // If the intro is still playing after waiting, force it to finish
        if (isIntroPlaying)
        {
            Debug.LogWarning("Animation event didn't trigger - forcing intro completion");
            OnIntroAnimationFinished();
        }
    }
    
    // This method should be called from the animation event
    public void OnIntroAnimationFinished()
    {
        // Prevent multiple calls
        if (!isIntroPlaying)
            return;
            
        Debug.Log("Boss intro animation finished");
        hasIntroPlayed = true;
        isIntroPlaying = false;
        
        // Show health bar HUD after intro
        if (bossHealthHUD != null)
        {
            bossHealthHUD.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Cannot show health HUD - reference is null");
        }
        
        // Allow the Animator to transition to next state
        if (animator != null)
            animator.SetTrigger("IntroFinished");
    }
    
    // Public method to manually trigger intro (useful for testing)
    [ContextMenu("Trigger Intro")]
    public void TriggerIntroManually()
    {
        if (!hasIntroPlayed && !isIntroPlaying)
        {
            StartIntroSequence();
        }
    }
    
    // Reset intro state (useful for testing)
    [ContextMenu("Reset Intro")]
    public void ResetIntro()
    {
        hasIntroPlayed = false;
        isIntroPlaying = false;
        if (bossHealthHUD != null)
            bossHealthHUD.SetActive(false);
    }
}