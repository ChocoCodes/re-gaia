using System.Collections;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [Header("Spike Trap Config")]
    [SerializeField] private int damage;
    [SerializeField] private float activeTime;
    [SerializeField] private float cd;

    private Animator animator;
    private bool isActive;
    private bool isTriggered;
    private float lastDamageTime = Mathf.NegativeInfinity;

    void Awake() {
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            Debug.Log("Spike Trap Triggered - Enter");

            // Start activation ONLY if not already triggered
            if (!isTriggered) {
                StartCoroutine(ActivateSpike());
            }

            // Damage player if active and cooldown passed
            if (isActive && Time.time - lastDamageTime >= cd) {
                Debug.Log("Player hit by Spike Trap (Enter)");
                InvokeSpikeDamage(other);
            }
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            // Start activation and animation if not triggered
            if (!isTriggered) {
                Debug.Log("Spike Trap Triggered - Stay, starting activation");
                StartCoroutine(ActivateSpike());
            }

            // Damage player if active and cooldown passed
            if (isActive && Time.time - lastDamageTime >= cd) {
                Debug.Log("Player hit by Spike Trap (Stay)");
                InvokeSpikeDamage(other);
            }
        }
    }

    void InvokeSpikeDamage(Collider2D other) {
        other.GetComponent<PlayerHealth>().TakeDamage(damage);
        lastDamageTime = Time.time;
    }

    IEnumerator ActivateSpike() {
        isTriggered = true;
        isActive = true;
        SoundManager.PlaySound(SoundType.SPIKE_ENABLED, 0.5f);
        animator.SetBool("activated", true);
        yield return new WaitForSeconds(activeTime);

        isActive = false;
        isTriggered = false;
        animator.SetBool("activated", false);
    }
}
