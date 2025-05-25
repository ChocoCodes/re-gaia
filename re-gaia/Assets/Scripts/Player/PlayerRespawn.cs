using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour 
{
    private Vector2 startPos;
    [SerializeField] private QuestManager questManager;
    [SerializeField] private GameObject bossFightRespawn;

    private void Start() {
        startPos = transform.position;
    }

    public IEnumerator Respawn(float duration) {
        yield return new WaitForSeconds(duration);
        // Go to the respawn position of the boss fight
        if(questManager && questManager.HasPlacedKey) {
            transform.position = bossFightRespawn.transform.position;
        } else {
            transform.position = startPos;
        }
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("idleRespawn");
        GetComponent<PlayerHealth>().InitHealth();
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<PlayerAttack>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        GetComponent<Rigidbody2D>().simulated = true;
    }
}