using UnityEngine;

public class MobDrop : MonoBehaviour
{
    [Header("Float Config")]
    private float speed = 2f;
    private float height = 0.25f;

    private Vector3 _startPos;

    void Start()
    {
        _startPos = transform.position;
    }

    void Update()
    {
        float offsetY = Mathf.Sin(Time.time * speed) * height;
        transform.position = _startPos + new Vector3(0, offsetY, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.CompareTag("Player")) {
            PlayerLootManager plm = collision.GetComponent<PlayerLootManager>();
            if(plm != null) {
                Debug.Log("Loot collected by player");
                plm.CollectLoot();
                Destroy(gameObject);
            }
        }
    }
}
