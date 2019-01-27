using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdibleOcean : Edible {
    public float shrinkScale;
    public float shrinkSpeed;

    bool drank = false;

    public override void Eat() {
        drank = true;
    }

    private void Update() {
        if (drank && transform.localScale.x > shrinkScale) {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * shrinkScale, shrinkSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other) {
        var chomnk = other.GetComponent<ChonkMotor>();
        var eater = other.GetComponent<Eater>();
        if (chomnk && eater && eater.curLevel < levelRequired && 
            Vector3.Dot(chomnk.rb.velocity, chomnk.transform.position) <= 0) {
            chomnk.Jump();
        }
    }
}
