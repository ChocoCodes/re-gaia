using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour 
{
    private Vector2 startPos;

    private void Start() {
        startPos = transform.position;
    }

    public IEnumerator Respawn(float duration) {
        yield return new WaitForSeconds(duration);
        transform.position = startPos;
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("idleRespawn");
        GetComponent<PlayerHealth>().InitHealth();
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAttack>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
    }
}