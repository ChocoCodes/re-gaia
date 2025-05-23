using UnityEngine;

public class QuestUIRenderer : MonoBehaviour {
    public SpriteRenderer spRenderer;
    [SerializeField] private Sprite[] progressSprites;
    [SerializeField] private Animator animator;
    [SerializeField] private QuestManager qm;

    void Start() {
        spRenderer.sprite = progressSprites[0];
        animator.enabled = false;
    }

    void Update() {
        if(qm.hasQuestStarted && !qm.hasQuestCompleted) {
            UpdateSprite();
            return;
        }
        if(qm.hasQuestCompleted) {
            //spRenderer.enabled = false;
            animator.enabled = true;
            animator?.SetTrigger("QuestCompleted");
            return;
        }
    }

    void UpdateSprite() {
        float progress = (float) qm.LOOT_COLLECTED / qm.LOOT_NEEDED;
        spRenderer.enabled = true;
        spRenderer.sprite = GetProgressSprite(progress);
    }

    Sprite GetProgressSprite(float progress) {
        if (progress >= 0.75f) { return progressSprites[3]; }
        if (progress >= 0.50f) { return progressSprites[2]; }
        if (progress >= 0.25f) { return progressSprites[1]; }
        else { return progressSprites[0]; }
    }
}