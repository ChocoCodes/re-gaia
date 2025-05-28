using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ProjectileSfx : MonoBehaviour
{
    private AudioSource projectileSource;

    void Awake()
    {
        projectileSource = GetComponent<AudioSource>();
    }
 // Animation events onl y
    public void PlayProjectileSfx()
    {
        projectileSource.clip = SoundManager.GetClip(SoundType.BOSS_PROJECTILE);
        projectileSource.volume = 0.1f;
        projectileSource.pitch = Random.Range(0.9f, 1.1f);
        projectileSource.Play();
    }

    public void PlayExplosionSfx()
    {
        // Stop projectile sound before playing explosion
        if (projectileSource.isPlaying)
            projectileSource.Stop();

        SoundManager.PlaySound(SoundType.BOSS_PROJECTILE_EXPLOSION, 0.3f);
    }
}
