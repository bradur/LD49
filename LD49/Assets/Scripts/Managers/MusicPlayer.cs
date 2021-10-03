using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer main;
    private void Awake() {
        main = this;
    }
    private List<AudioFade> fades = new List<AudioFade>();

    [SerializeField]
    private AudioSource musicSource = null;

    [SerializeField]
    private float volume = 0.8f;

    [SerializeField]
    private float fadeOutDuration = 0.2f;

    public void PlayMusic(AudioClip newTrack) {
        if (!musicSource.isPlaying) {
            musicSource.clip = newTrack;
            musicSource.volume = volume;
            musicSource.Play();
        } else {
            fades.Add(new AudioFade(fadeOutDuration, 0f, musicSource, delegate {
                musicSource.Stop();
                PlayMusic(newTrack);
            }));
        }
    }

    public void Update() {
        for(int index = 0; index < fades.Count; index += 1) {
            AudioFade fade = fades[index];
            if (fade != null && fade.IsFading) {
                fade.Update();
            }
            if (!fade.IsFading) {
                fades.Remove(fade);
            }
        }
    }
}

public class AudioFade {
    public AudioFade(float duration, float target, AudioSource track, UnityAction callback) {
        this.duration = duration;
        IsFading = true;
        timer = 0f;
        originalVolume = track.volume;
        targetVolume = target;
        audioSource = track;
        fadeComplete = callback;
    }
    public bool IsFading {get; private set;}
    private float duration;
    private float timer;
    private float targetVolume;
    private AudioSource audioSource;
    private float originalVolume;

    private UnityAction fadeComplete;

    public void Update() {
        timer += Time.unscaledDeltaTime / duration;
        audioSource.volume = Mathf.Lerp(originalVolume, targetVolume, timer);
        if (timer >= 1) {
            audioSource.volume = targetVolume;
            IsFading = false;
            fadeComplete.Invoke();
        }
    }
}