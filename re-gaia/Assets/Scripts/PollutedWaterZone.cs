using UnityEngine;

public class PollutedWaterZone : MonoBehaviour
{
    [Header("Polluted Water Zone Attributes")]
    public int damagePerSec = 3;
    public float slowMultiplier = 0.3f;
    public float timer = 1f; // Wait for 1s before applying damage
    public float zoneFallSpeed = 0.2f;

    private PlayerHealth _health;
    private PlayerMovement _movement;   
    private bool _isInZone = false;
    private float _dmgTimer = 0f;
    private float _cachedFallSpeed; // Store original fallSpeedMultiplier for reset

    // Player enters the zone - initialize state and apply slow
    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Player entered the zone");
        if(other.CompareTag("Player")) {
            _isInZone = true;
            _health = other.GetComponent<PlayerHealth>();
            _movement = other.GetComponent<PlayerMovement>();

            if(_movement != null) {
                _movement.extSpeedMultiplier = slowMultiplier;
                _cachedFallSpeed = _movement.fallSpeedMultiplier; // Store original fallSpeedMultiplier
                _movement.fallSpeedMultiplier = zoneFallSpeed;
                Debug.Log($"Current spd {_movement.extSpeedMultiplier} | Gravity: {_movement.baseGravity}");
            }

            _dmgTimer = 0f;
        }
    }

    // Apply damage, pull force, and slow effect to the player while in the zone over time
    private void Update() {
        if(_isInZone && _health != null) {
            Debug.Log("Player is in the zone");
            _dmgTimer += Time.deltaTime;
            if (_dmgTimer >= timer) {
                _health.TakeDamage(damagePerSec);
                _dmgTimer = 0f; // Reset timer after applying damage
                Debug.Log($"Damage applied: {damagePerSec} Current health {_health.GetCurrentHealth()}");
            }
        }
    }

    // Reset everything to default when player exits the zone
    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log("Player exited the zone");
        if(other.CompareTag("Player")) {
            _isInZone = false;
            _health = null;
            // Reset the player's movement speed and fall speed to original
            if(_movement != null) { 
                _movement.extSpeedMultiplier = 1f;
                _movement.fallSpeedMultiplier = _cachedFallSpeed;
            }
            _movement = null;
        }
    }
}
