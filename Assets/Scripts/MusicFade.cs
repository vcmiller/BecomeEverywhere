using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicFade : MonoBehaviour {
    private AudioSource src;
    private float volume;

    public float fadeDuration;
    public float targetFadeLevel { get; set; }

    // Start is called before the first frame update
    void Start() {
        src = GetComponent<AudioSource>();
        volume = src.volume;
        targetFadeLevel = src.playOnAwake ? 1 : 0;
        src.volume = targetFadeLevel * volume;
    }

    private void Update() {
        src.volume = Mathf.MoveTowards(src.volume, targetFadeLevel * volume, volume / fadeDuration);
        if (src.volume > 0 && !src.isPlaying) {
            src.Play();
        } else if (src.volume == 0 && src.isPlaying) {
            src.Stop();
        }
    }
}
