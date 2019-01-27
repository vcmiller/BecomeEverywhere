using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMeRightRound : MonoBehaviour {
    public Vector3 axis;
    public float rotationRate;
    
    private void Update() {
        transform.Rotate(axis, rotationRate * Time.deltaTime, Space.World);
    }
}
