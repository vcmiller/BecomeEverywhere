using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleView : MonoBehaviour {
    private float near, far;
    private Camera cam;

    // Start is called before the first frame update
    void Start() {
        cam = GetComponent<Camera>();
        near = cam.nearClipPlane;
        far = cam.farClipPlane;
    }

    // Update is called once per frame
    void Update() {
        float s = transform.root.localScale.x;
        cam.nearClipPlane = near * s;
        cam.farClipPlane = far * s;
    }
}
