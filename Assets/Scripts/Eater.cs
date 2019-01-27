using SBR;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour {
    private ChonkMotor motor;
    private float destSize;
    private ExpirationTimer hitSlowTimer;
    public float eaten { get; private set; }
    public float growSpeed = 1;

    public float hitSlowDuration = 0.2f;
    public float hitSlowAmount = 0.3f;

    public int curLevel = 0;
    public LevelList levels;

    public AudioParameters eatSound;
    public AudioParameters levelUpSound;

    public static event Action<int> LevelUp;
    
    [System.Serializable]
    public struct Level {
        public float threshold;
        public float size;
    }

    [System.Serializable]
    public class LevelList : DraggableList<Level> { }

    // Start is called before the first frame update
    void Start() {
        motor = GetComponentInParent<ChonkMotor>();
        destSize = levels[curLevel].size;
        eaten = levels[curLevel].threshold;
        hitSlowTimer = new ExpirationTimer(hitSlowDuration);
        hitSlowTimer.unscaled = true;

        if (curLevel > 0) {
            LevelUp?.Invoke(curLevel);
        }
    }

    private void Update() {
        motor.size = Mathf.MoveTowards(motor.size, destSize, Time.deltaTime * growSpeed * motor.size);

        if (!hitSlowTimer.expired) {
            Time.timeScale = Mathf.Lerp(1, hitSlowAmount, hitSlowTimer.remainingRatio * 2);
        } else {
            Time.timeScale = 1;
        }
    }

    public float GetNextLevelProgress() {
        if (curLevel == levels.length - 1) {
            return 0;
        }

        float f = eaten - levels[curLevel].threshold;
        float needed = levels[curLevel + 1].threshold - levels[curLevel].threshold;
        return f / needed;
    }

    private void OnTriggerEnter(Collider other) {
        var edible = other.GetComponent<Edible>();
        if (edible && curLevel >= edible.levelRequired) {
            if (edible.levelRequired == curLevel) {
                hitSlowTimer.Set();
            }

            edible.Eat();
            eatSound.pitch = UnityEngine.Random.Range(0.9f, 1.3f);

            float f = eatSound.volume;
            eatSound.volume *= Mathf.Clamp01((edible.levelRequired + 4 - curLevel) / 4.0f);
            if (eatSound.volume > 0) {
                eatSound.PlayAtPoint(transform.position);
            }
            eatSound.volume = f;

            Eat(edible.sizeGained);
        }
    }

    public void Eat(float amount) {
        eaten += amount;

        if (curLevel < levels.length - 1 && eaten >= levels[curLevel + 1].threshold) {
            curLevel++;
            LevelUp?.Invoke(curLevel);
            levelUpSound.PlayAtPoint(transform.position);
            destSize = levels[curLevel].size;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        OnTriggerEnter(collision.collider);
    }
}
