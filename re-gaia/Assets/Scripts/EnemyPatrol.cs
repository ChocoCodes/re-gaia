using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("EnemyPatrol")]
    public float patrolSpeed;
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D rb;
    public Transform currentPoint;

    // Update is called once per frame
    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;

        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(patrolSpeed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-patrolSpeed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            Flip();
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            Flip();
            currentPoint = pointB.transform;
        }
    }

    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
