using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollSound : MonoBehaviour {
    private AudioSource src;
    private ChonkMotor motor;
    public float fadeSpeed = 4;

    private float startVolume;

    private void Start() {
        src = GetComponent<AudioSource>();
        motor = GetComponentInParent<ChonkMotor>();

        startVolume = src.volume;
        src.volume = 0;
    }

    private void Update() {
        if (!motor.grounded && src.isPlaying) {
            src.volume = Mathf.MoveTowards(src.volume, 0, Time.deltaTime * fadeSpeed);
        } else if (motor.grounded) {
            float destVolume = startVolume * motor.rb.velocity.magnitude / motor.moveSpeed;
            src.volume = Mathf.MoveTowards(src.volume, destVolume, Time.deltaTime * fadeSpeed);
        }
    }
}
