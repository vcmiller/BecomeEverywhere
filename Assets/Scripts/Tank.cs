using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {
    private Eater chonk;

    public float moveSpeed = 10;
    public float rotateSpeed = 90;

    public float raycastDistance;
    public float offset = 2.5f;

    public float minDist = 20;

    // Start is called before the first frame update
    void Start() {
        chonk = FindObjectOfType<Eater>();

        GetComponent<Edible>().UpdateLevel(chonk.curLevel);
    }

    // Update is called once per frame
    void Update() {
        if (Physics.Raycast(
            transform.position + transform.up * raycastDistance,
            -transform.up,
            out RaycastHit hit,
            raycastDistance * 2, 1)) {
            transform.position = hit.point + hit.normal * offset;
            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;

            Vector3 toPlayer = chonk.transform.position - transform.position;
            Vector3 c = Vector3.Cross(transform.up, toPlayer);
            c = Vector3.Cross(c, transform.up);
            if (c.sqrMagnitude > 0) {
                Quaternion targetRot = Quaternion.LookRotation(c, transform.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, chonk.transform.position) > minDist) {
                transform.Translate(0, 0, moveSpeed * Time.deltaTime, Space.Self);
            }
        }
    }
}
