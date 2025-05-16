using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Sanctuary Enemy Attack")]
    public int damage = 5;
    public float lifetime = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Vector2 direction = Vector2.right * Mathf.Sign(transform.localScale.x);
        //rb.linearVelocity = transform.right  * speed;
        Destroy(gameObject, lifetime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerHealth player = collision.GetComponent<PlayerHealth>();
        if (player)
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
