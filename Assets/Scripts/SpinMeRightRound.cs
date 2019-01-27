using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMeRightRound : MonoBehaviour {
    public Vector3 axis;
    public float rotationRate;

    private Material skyboxMat;

    private void Start() {
        skyboxMat = RenderSettings.skybox;
    }

    private void Update() {
        transform.Rotate(axis, rotationRate * Time.deltaTime, Space.World);
        skyboxMat.SetFloat("_Rotation", -transform.localEulerAngles.y);
    }
}
