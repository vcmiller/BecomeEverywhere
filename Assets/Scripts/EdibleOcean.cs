using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdibleOcean : MonoBehaviour {
    public float shrinkScale;
    public float shrinkSpeed;
    public float eatRate = 2000;
    public int levelRequired = 7;
    
    private void OnTriggerStay(Collider other) {
        var chomnk = other.GetComponent<ChonkMotor>();
        var eater = other.GetComponent<Eater>();
        if (chomnk && eater) {
            if (eater.curLevel < levelRequired &&
                Vector3.Dot(chomnk.rb.velocity, chomnk.transform.position) <= 0) {
                chomnk.Jump();
            } else {
                transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * shrinkScale, shrinkSpeed * Time.deltaTime);
                eater.Eat(eatRate * Time.deltaTime);
            }
        }
    }
}
