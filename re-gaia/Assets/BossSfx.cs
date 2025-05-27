using UnityEngine;

public class BossSfx : MonoBehaviour
{
    // Animation Events only
    public void PlayWalkSFX()
    {
        SoundManager.PlaySound(SoundType.BOSS_WALK, 0.6f);
    }
    public void PlayAtkSFX()
    {
        SoundManager.PlaySound(SoundType.BOSS_ATK, 0.7f);
    }
}
