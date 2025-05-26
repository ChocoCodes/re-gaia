using UnityEngine;
using UnityEngine.UI;

public class PlayerSlash : MonoBehaviour
{
    public Slider slider;
    public Image skillImage;
    public Animator playerAnimator;
    public PlayerMovement movement;

    [Header("Slash")]
    public float cooldown = 10f;
    public float slashSpeed = 10f;
    public Transform slashPoint;
    public bool isUnlocked = false;
    public GameObject slashPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.maxValue = cooldown;
        slider.value = cooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (slider.value < slider.maxValue) slider.value += Time.deltaTime;

        if (slider.value == slider.maxValue && isUnlocked) skillImage.color = Color.white;
    }

    public void Slash()
    {
        if (!isUnlocked) return;
        if (slider.value < slider.maxValue) return;

        movement.SetCanMove(false);

        playerAnimator.SetTrigger("attack");

        float direction = transform.localScale.x > 0 ? 1f : -1f;

        GameObject slash = Instantiate(slashPrefab, slashPoint.position, Quaternion.identity);

        Vector3 slashScale = slash.transform.localScale;
        slashScale.x = Mathf.Abs(slashScale.x) * direction;
        slash.transform.localScale = slashScale;

        Rigidbody2D rb = slash.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(direction * slashSpeed, 0f);
        }

        slider.value = 0f;
        skillImage.color = Color.gray;
    }
}
