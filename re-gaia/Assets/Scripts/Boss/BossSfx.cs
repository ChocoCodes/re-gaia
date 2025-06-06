using UnityEngine;

public class BossSfx : MonoBehaviour
{
    // Animation Events only
    public void PlayWalkSFX()
    {
        SoundManager.PlaySound(SoundType.BOSS_WALK, 0.4f);
    }
    public void PlayAtkSFX()
    {
        SoundManager.PlaySound(SoundType.BOSS_ATK, 0.7f);
    }

    public void PlayDeathSFX()
    {
        SoundManager.PlaySound(SoundType.BOSS_DEATH, 1f);
    }

    public void PlayGrowlSFX()
    {
        SoundManager.PlaySound(SoundType.BOSS_GROWL, 0.7f);
    }

    public void PlayIntroSFX()
    {
        SoundManager.PlaySound(SoundType.BOSS_INTRO, 0.6f);
    }

    public void PlayTakeHitSFX()
    {
        SoundManager.PlaySound(SoundType.BOSS_TAKE_HIT, 0.45f);
        SoundManager.PlaySound(SoundType.ENEMY_TAKE_HIT, 0.7f);
    }
}
