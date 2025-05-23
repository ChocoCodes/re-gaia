using UnityEngine;
using System.Collections;

public class BossIntroController : MonoBehaviour
{
    public float triggerDistance = 5f; // Distance at which intro starts
    public Animator animator;
    public HealthBar healthBar;
    public Transform player;
    public GameObject bossHealthHUD; // Reference to the Canvas GameObject

    public bool hasIntroPlayed = false;
    private bool isIntroPlaying = false;

    void Start()
    {
        // Find Canvas if not assigned
        if (bossHealthHUD == null)
        {
            // Look for the Canvas in children
            bossHealthHUD = transform.Find("Canvas")?.gameObject;
            
            if (bossHealthHUD == null)
                Debug.LogError("Canvas GameObject could not be found as a child!");
        }

        // Make sure all references are assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            Debug.LogWarning("Animator was not assigned - attempting to get from this GameObject");
        }
        
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
            Debug.LogWarning("Player was not assigned - attempting to find Player");
        }

        if (healthBar == null)
        {
            // Try to find health bar in the Canvas
            if (bossHealthHUD != null)
                healthBar = bossHealthHUD.GetComponentInChildren<HealthBar>();
                
            if (healthBar == null)
                Debug.LogError("HealthBar reference is missing!");
        }

        // Hide the HUD at start
        if (bossHealthHUD != null)
        {
            bossHealthHUD.SetActive(false);
        }
        else
        {
            Debug.LogError("Canvas/HUD reference is missing!");
        }
    }

    void Update()
    {
        if (!hasIntroPlayed && !isIntroPlaying && player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            Debug.Log($"Distance to player: {distance}, Trigger distance: {triggerDistance}");
            
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
            
            // Alternative approach using coroutine as a backup
            // in case animation events aren't working
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
        // You may need to adjust this time to match your animation length
        yield return new WaitForSeconds(3f);
        
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
}