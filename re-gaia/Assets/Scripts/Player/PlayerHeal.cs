using UnityEngine;
using UnityEngine.UI;

public class PlayerHeal : MonoBehaviour
{
    public Slider slider;
    public PlayerHealth health;
    public Image skillImage;
    public Animator animator;

    [Header("Heal")]
    public float cooldown = 10f;
    public int healAmount = 30;

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

        if (slider.value == slider.maxValue) skillImage.color = Color.white;
    }

    public void Heal()
    {
        if (slider.value < slider.maxValue) return;
        if (health.GetCurrentHealth() == health.maxHealth) return;
        if (health.GetCurrentHealth() <= 0) return;

        animator.SetTrigger("heal");
        slider.value = 0f;
        skillImage.color = Color.gray;
        health.HealPlayer(healAmount);
        SoundManager.PlaySound(SoundType.PLAYER_HEAL, 0.5f);
    }
}
