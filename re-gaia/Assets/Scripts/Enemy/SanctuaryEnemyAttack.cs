using UnityEngine;

public class SanctuaryEnemyAttack : MonoBehaviour
{
    public Animator animator;
    public bool isDead;

    [Header("Sanctuary Enemy Attack")]
    public float detectionRange = 5f;
    public float bulletSpeed = 10f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public LayerMask playerLayer;
    public EnemyPatrol patrol;
    public bool isAttacking;
    Vector2 direction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        direction = transform.right * Mathf.Sign(transform.localScale.x);
        isDead = animator.GetBool("isDead");
        
        this.enabled = isDead ? false : true;

        patrol.isPaused = PlayerInSight() || isAttacking;

        if (PlayerInSight() && !isAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("attack");
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, detectionRange, playerLayer);
        if (hit.collider != null && hit.collider.CompareTag("Player"))
        {
            return true;
        }
        return false;
    }

    public void ShootAtPlayer()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = direction * bulletSpeed;
    }

    public void PlayFormingSFX() {
        SoundManager.PlaySound(SoundType.ENEMY_PROJ_FORMING, 0.7f);
    }

    public void PlayLaunchSFX() {
        SoundManager.PlaySound(SoundType.ENEMEY_PROJ_LAUNCH, 0.7f);
    }

    public void PlayExplodeSFX() {
        SoundManager.PlaySound(SoundType.ENEMY_PROJ_EXPLODE, 0.7f);
    }
    public void EndAttack()
    {
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.left * transform.localScale.x * detectionRange);
    }
}
