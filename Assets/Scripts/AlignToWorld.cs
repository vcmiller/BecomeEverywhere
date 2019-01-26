using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlignToWorld : MonoBehaviour {
    private void Update() {
        if (transform.position.sqrMagnitude > 0) {
            transform.up = transform.position;
        }
    }
}
