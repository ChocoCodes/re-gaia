using UnityEngine;

public class SlashPrefab : MonoBehaviour
{
    public Animator animator;
    private float destroyDelay = 0.25f;

    [Header("Slash Attack")]
    public int damage = 30;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.TakeDamage(damage);
            animator.SetTrigger("hit");
            StartCoroutine(DestroyAfterAnimation());
        }

        Boss_Health bossHealth = collision.GetComponent<Boss_Health>();
        if (bossHealth)
        {
             bossHealth.TakeDamage(damage);
             animator.SetTrigger("hit");
             StartCoroutine(DestroyAfterAnimation());
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Destroy(gameObject);
        }
    }

    private System.Collections.IEnumerator DestroyAfterAnimation()
    {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
