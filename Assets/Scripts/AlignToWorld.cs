using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AlignToWorld : MonoBehaviour {
    public bool randomScale = true;

    [Conditional("randomScale")]
    public float scaleMin = 0.7f;

    [Conditional("randomScale")]
    public float scaleMax = 1.5f;

    public bool randomAngle = true;
    public bool useNormal = false;

#if UNITY_EDITOR
    private void Awake() {
        if (Application.isPlaying) {
            Destroy(this);
        }
    }

    private void Start() {
        if (randomScale) {
            transform.localScale = Vector3.one * Random.Range(scaleMin, scaleMax);
        }

        if (randomAngle) {
            transform.eulerAngles = new Vector3(0, Random.Range(0, 360), 0);
        }
    }

    private void Update() {
        if (transform.position.sqrMagnitude > 0) {
            Vector3 u = transform.position.normalized;
            if (useNormal) {
                RaycastHit hit;
                if (Physics.Raycast(transform.position +  u * 5, -u, out hit, 10, 1)) {
                    u = hit.normal;
                }
            }

            var d = Vector3.Dot(transform.up, u);
            if (d < 0.995f) {
                transform.rotation = Quaternion.FromToRotation(transform.up, u) * transform.rotation;
            }
        }
    }
#endif
}
