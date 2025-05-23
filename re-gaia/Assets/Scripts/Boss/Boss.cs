// Boss.cs - Fixed IsInSkillRange method and added helper method
using UnityEngine;
public class Boss : MonoBehaviour
{
    [Header("Basic Attack Settings")]
    public float basicAttackCooldown = 2f;
    private float basicLastAttackTime;
    public float basicAttackRange = 7f;
    public int basicAttackdamage = 10;
    public float basicKnockbackForce = 10f;
    public float basicKnockbackDuration = 0.2f;
    public bool basicWaitingForCooldown = false;

    [Header("Skill Attack Settings")]
    public float skillCooldown = 3f;
    private float skillLastAttackTime;
    public float skillTriggerRange = 15f; // Must be greater than basicAttackRange
    public int skillDamage = 25;
    public float skillKnockbackForce = 20f;
    public float skillKnockbackDuration = 0.5f;
    public bool skillWaitingForCooldown = false;

    [Header("General Settings")]
    public Transform player;
    public bool isFlipped = false;

    public bool IsAttackOnCooldown()
    {
        return Time.time < basicLastAttackTime + basicAttackCooldown;
    }

    public void ResetAttackCooldown()
    {
        basicLastAttackTime = Time.time;
    }

    public bool IsSkillOnCooldown()
    {
        return Time.time < skillLastAttackTime + skillCooldown;
    }

    public void ResetSkillCooldown()
    {
        skillLastAttackTime = Time.time;
    }

    // Fixed: This should return true when player is far enough for skill attack
    public bool IsInSkillRange()
    {
        if (player == null) return false;
        float horizontalDistance = Mathf.Abs(player.position.x - transform.position.x);
        return horizontalDistance >= skillTriggerRange;
    }

    // Helper method to get current distance to player
    public float GetDistanceToPlayer()
    {
        if (player == null) return float.MaxValue;
        return Mathf.Abs(player.position.x - transform.position.x);
    }

    public bool HasIntroPlayed
    {
        get
        {
            return GetComponent<BossIntroController>()?.hasIntroPlayed ?? true;
        }
    }

    public void LookAtPlayer()
    {
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;

        if (transform.position.x > player.position.x && isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = false;
        }
        else if (transform.position.x < player.position.x && !isFlipped)
        {
            transform.localScale = flipped;
            transform.Rotate(0f, 180f, 0f);
            isFlipped = true;
        }
    }
}