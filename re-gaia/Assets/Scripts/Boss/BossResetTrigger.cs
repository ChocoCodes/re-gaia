using UnityEngine;

public class BossResetTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasTriggered)
        {
            hasTriggered = true;
            return;
        }

        if (other.CompareTag("Player"))
        {
            GameObject bossObj = GameObject.FindGameObjectWithTag("Boss");
            if (bossObj != null)
            {
                Boss boss = bossObj.GetComponent<Boss>();
                if (boss != null)
                {
                    boss.ResetBoss();
                    Debug.Log("[BossResetTrigger] Player triggered boss reset.");
                }
            }
            else
            {
                Debug.LogWarning("[BossResetTrigger] No GameObject with tag 'Boss' found.");
            }
        }
    }
}
