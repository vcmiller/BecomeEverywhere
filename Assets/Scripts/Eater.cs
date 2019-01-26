using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eater : MonoBehaviour {
    private ChonkMotor motor;
    private float destSize;
    public float growSpeed = 1;

    // Start is called before the first frame update
    void Start() {
        motor = GetComponentInParent<ChonkMotor>();
        destSize = motor.size;
    }

    private void Update() {
        motor.size = Mathf.MoveTowards(motor.size, destSize, Time.deltaTime * growSpeed * motor.size);

        if (Input.GetButtonDown("Fire1")) {
            destSize *= 1.2f;
        }
    }

    private void OnCollisionEnter(Collision collision) {
        var edible = collision.collider.GetComponent<Edible>();
        if (edible && motor.size >= edible.sizeRequired) {
            edible.Eat();
            destSize += edible.sizeGained;
        }
    }
}
