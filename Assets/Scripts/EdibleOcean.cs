using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdibleOcean : MonoBehaviour {
    public float shrinkScale;
    public float shrinkSpeed;
    public float eatRate = 2000;
    public int levelRequired = 7;
    public AudioParameters splashSound;
    public AudioParameters screamSound;

    public AudioSource slurpSound;
    private float slurpVolume;
    public float slurpFadeSped = 2;
    public float drinkExppiration = 1;
    private ExpirationTimer drinkTimer;

    private void Start() {
        slurpVolume = slurpSound.volume;
        slurpSound.volume = 0;
        drinkTimer = new ExpirationTimer(drinkExppiration);
    }

    private void OnTriggerStay(Collider other) {
        var chomnk = other.GetComponent<ChonkMotor>();
        var eater = other.GetComponent<Eater>();
        if (chomnk && eater) {
            if (eater.curLevel < levelRequired) {
                if (Vector3.Dot(chomnk.rb.velocity, chomnk.transform.position) <= 0) {
                    chomnk.Jump();
                    splashSound.PlayAtPoint(chomnk.transform.position);
                    screamSound.PlayAtPoint(chomnk.transform.position);
                }
            } else {
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * shrinkScale, shrinkSpeed * Time.deltaTime);
                eater.Eat(eatRate * Time.deltaTime);
                drinkTimer.Set();
            }
        }
    }

    private void LateUpdate() {
        if (!drinkTimer.expired) {
            slurpSound.volume = Mathf.MoveTowards(slurpSound.volume, slurpVolume, Time.deltaTime * slurpFadeSped);
        } else {
            slurpSound.volume = Mathf.MoveTowards(slurpSound.volume, 0, Time.deltaTime * slurpFadeSped);
        }
    }
}
