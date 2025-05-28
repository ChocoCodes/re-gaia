using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    PLAYER_SLASH,
    PLAYER_HEAL,
    PLAYER_HURT,
    PLAYER_FOOTSTEP,
    PLAYER_JUMP,
    PLAYER_DEATH,
    PLAYER_ATTACK,
    PLAYER_DASH,
    SPIKE_ENABLED,
    BARRIER_DESTROYED,
    ITEM_PICKUP,
    BOSS_WALK,
    BOSS_ATK,
    BOSS_PROJECTILE,
    BOSS_PROJECTILE_EXPOLOSION,
    BOSS_PROJECTILE_LAUNCH,
    BOSS_DEATH,
    BOSS_GROWL,
    BOSS_INTRO,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager: MonoBehaviour 
{
    [SerializeField] private List<SoundList> soundList = new List<SoundList>();
    private static SoundManager instance;
    private AudioSource asrc;

    void Awake() {
        instance = this;
        asrc = GetComponent<AudioSource>();
    }

    // Returns a random pitch value between min and max
    float GetRandomPitch(float min = 0.9f, float max = 1.1f) => UnityEngine.Random.Range(min, max);
    public static void PlaySound(SoundType sound, float volume = 1f) {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip clip = clips.Length > 1 ? clips[UnityEngine.Random.Range(0, clips.Length)] : clips[0]; 
        instance.asrc.pitch = instance.GetRandomPitch();
        instance.asrc.PlayOneShot(clip, volume);
        // Reset pitch to default after playing
        instance.asrc.pitch = 1f; 
    }

    public static AudioClip GetClip(SoundType sound)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        return clips.Length > 1 ? clips[UnityEngine.Random.Range(0, clips.Length)] : clips[0];
    }

    #if UNITY_EDITOR
        private void OnEnable() {
            string[] names = Enum.GetNames(typeof(SoundType));
            Debug.Log("SoundManager: " + names.Length + " sound types found.");
            while (soundList.Count < names.Length)
                soundList.Add(new SoundList());
            for (int i = 0; i < names.Length; i++) {
                soundList[i].audioSetName = names[i];
            }
        }
    #endif
}

[Serializable]
public class SoundList {
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string audioSetName;
    [SerializeField] private AudioClip[] sounds;
}