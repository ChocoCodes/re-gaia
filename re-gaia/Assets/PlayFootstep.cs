using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    public void PlaySound() {
        SoundManager.PlaySound(SoundType.PLAYER_FOOTSTEP, 0.7f);
    }
}
