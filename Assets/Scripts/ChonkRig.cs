using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChonkRig : MonoBehaviour {
    public Transform stabilizer;
    public Transform sphere;
    public Transform squash;
    public Transform groundAlign;

    public Transform[] limbsAndSuch;

    public float airVelStretchFactor = 1f;
    public float squashSpeed = 0.3f;
    public float groundAlignSpeed = 360;

    public AnimationCurve squashCurve;

    public float squashAmount { get; private set; }

    public ChonkMotor motor { get; private set; }

    private (Vector3 pos, Quaternion rot)[] limbTransforms;
    private Quaternion lastRotation;

    private void Awake() {
        motor = GetComponent<ChonkMotor>();
        squashAmount = 1;

        limbTransforms = new (Vector3, Quaternion)[limbsAndSuch.Length];
        for (int i = 0; i < limbsAndSuch.Length; i++) {
            limbTransforms[i].pos = sphere.InverseTransformPoint(limbsAndSuch[i].position);
            limbTransforms[i].rot = Quaternion.Inverse(sphere.rotation) * limbsAndSuch[i].rotation;
        }

        lastRotation = stabilizer.rotation;
    }

    private void LateUpdate() {
        UpdateRotation();
        UpdateSquashTransforms();
        UpdateLimbs();
    }

    private void UpdateLimbs() {
        for (int i = 0; i < limbsAndSuch.Length; i++) {
            limbsAndSuch[i].position = sphere.TransformPoint(limbTransforms[i].pos);
            limbsAndSuch[i].rotation = sphere.rotation * limbTransforms[i].rot;
        }
    }

    private void UpdateRotation() {
        Vector3 oldUp = lastRotation * Vector3.up;
        Quaternion q = Quaternion.FromToRotation(oldUp, transform.position);
        stabilizer.rotation = q * lastRotation;
        lastRotation = stabilizer.rotation;
        sphere.rotation = Quaternion.LookRotation(transform.forward, transform.right);

        Vector3 up = transform.position;
        if (motor.grounded) {
            up = motor.groundHit.normal;
        }
        q = Quaternion.FromToRotation(Vector3.up, up);
        groundAlign.rotation = 
            Quaternion.RotateTowards(groundAlign.rotation, q, groundAlignSpeed * Time.deltaTime);
    }

    private void UpdateSquashAmount() {
        float targetSquash = 1;
        float velY = Vector3.Dot(motor.rb.velocity, transform.position.normalized);
        if (motor.grounded) {
            targetSquash = squashCurve.Evaluate(Time.time - motor.groundHitTime);
        } else if (velY < 0) {
            targetSquash = 1 + airVelStretchFactor * Mathf.Abs(velY) / motor.size;
        }
        squashAmount = Mathf.Lerp(squashAmount, targetSquash, squashSpeed);

    }

    private void UpdateSquashTransforms() {
        UpdateSquashAmount();

        float sqrt = Mathf.Sqrt(1 / squashAmount);
        squash.localScale = new Vector3(squashAmount, sqrt, sqrt);
        float radius = motor.sc.radius;

        float bottom = radius * (1.0f - squashAmount) / squashAmount;

        sphere.localPosition = new Vector3(bottom, 0, 0);
    }
}
