using UnityEngine;
using System.Collections;

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager instance;
    private AudioSource audioSrc;
    [Range(0f, 1f)] public float targetVolume = 0.2f;
    public float fadeDuration = 2f;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSrc = GetComponent<AudioSource>();
        } else { Destroy(gameObject); }
    }

    public void PlayMusic(AudioClip clip) {
        StopAllCoroutines();
        audioSrc.clip = clip;
        audioSrc.volume = 0f;
        audioSrc.loop = true;
        audioSrc.Play();
        StartCoroutine(FadeInMusic());
    }

    IEnumerator FadeInMusic() {
        float currTime = 0f;

        while (currTime < fadeDuration) {
            currTime += Time.deltaTime;
            audioSrc.volume = Mathf.Lerp(0f, targetVolume, currTime / fadeDuration);
            Debug.Log($"Volume: {audioSrc.volume}");
            yield return null;
        }

        audioSrc.volume = targetVolume;
    }
}
