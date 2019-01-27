using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour {
    private ChonkMotor motor;
    private float destSize;
    public float eaten { get; private set; }
    public float growSpeed = 1;

    public int curLevel = 0;
    public LevelList levels;
    
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
    }

    private void Update() {
        motor.size = Mathf.MoveTowards(motor.size, destSize, Time.deltaTime * growSpeed * motor.size);
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
            edible.Eat();
            eaten += edible.sizeGained;

            if (curLevel < levels.length - 1 && eaten >= levels[curLevel + 1].threshold) {
                curLevel++;
                destSize = levels[curLevel].size;
            }
        }
    }

    private void OnCollisionEnter(Collision collision) {
        OnTriggerEnter(collision.collider);
    }
}
