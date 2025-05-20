using UnityEngine;

public class PollutedWaterZone : MonoBehaviour
{
    [Header("Polluted Water Zone Attributes")]
    public int damagePerSec = 3;
    public float slowMultiplier = 0.3f;
    public float timer = 1f;
    public float zoneFallSpeed = 0.2f;

    private PlayerHealth _health;
    private PlayerMovement _movement;
    private float _dmgTimer = 0f;
    private float _cachedFallSpeed;
    private bool _playerInZone = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player entered the zone");
        if (!other.CompareTag("Player")) return;

        _health = other.GetComponent<PlayerHealth>();
        _movement = other.GetComponent<PlayerMovement>();

        if (_movement != null)
        {
            _cachedFallSpeed = _movement.fallSpeedMultiplier; // Cache original fall speed
        }

        _playerInZone = true;
        _dmgTimer = 0f;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("Player is in the zone");
        if (!_playerInZone || !other.CompareTag("Player")) return;

        if (_movement != null)
        {
            _movement.extSpeedMultiplier = slowMultiplier;
            _movement.fallSpeedMultiplier = zoneFallSpeed;
        }

        if (_health != null)
        {
            _dmgTimer += Time.deltaTime;
            if (_dmgTimer >= timer)
            {
                _health.TakeDamage(damagePerSec);
                _dmgTimer = 0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_playerInZone || !other.CompareTag("Player")) return;
        Debug.Log("Player exited the zone");
        if (_movement != null)
        {
            _movement.extSpeedMultiplier = 1f;
            _movement.fallSpeedMultiplier = _cachedFallSpeed;  // Reset to original value
        }

        _health = null;
        _movement = null;
        _playerInZone = false;
        _dmgTimer = 0f;
    }
}
