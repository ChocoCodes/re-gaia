using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public bool isPaused = false;
    public bool isChasing = false;

    [Header("EnemyPatrol")]
    public float patrolSpeed;
    public GameObject pointA;
    public GameObject pointB;
    public Rigidbody2D rb;
    public Transform currentPoint;

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isChasing)
        {
            return;
        }

        Vector2 point = currentPoint.position - transform.position;

        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(patrolSpeed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-patrolSpeed, 0);
        }

        FaceByVelocity();

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f)
        {
            currentPoint = (currentPoint == pointB.transform) ? pointA.transform : pointB.transform;
        }
/*
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            Flip();
            currentPoint = pointA.transform;
        }
        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            Flip();
            currentPoint = pointB.transform;
        }*/
    }
/*
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
    */
    private void FaceByVelocity()
    {
        float vx = rb.linearVelocity.x;
        if (Mathf.Abs(vx) < 0.01f) return;

        bool movingRight = vx > 0f;
        bool facingRight = transform.localScale.x > 0f;

        if (movingRight != facingRight)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
