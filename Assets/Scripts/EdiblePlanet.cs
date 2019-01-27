using SBR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdiblePlanet : MonoBehaviour {
    public int levelRequired = 8;
    public GameObject edibleRoot;
    public float particleDuration = 5;
    public float reqVel = 50;
    public AudioParameters eatSound;
    private ParticleSystem ps;

    private void OnCollisionEnter(Collision collision) {
        var chomnk = collision.collider.GetComponent<ChonkMotor>();
        var eater = collision.collider.GetComponent<Eater>();
        if (collision.relativeVelocity.sqrMagnitude > reqVel * reqVel && 
            chomnk && eater && eater.curLevel >= levelRequired) {
            Destroy(edibleRoot);
            ps.transform.parent = null;
            ps.Play();
            eatSound.PlayAtPoint(transform.position);
            Destroy(ps, particleDuration);
            Destroy(transform.parent.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        ps = GetComponentInChildren<ParticleSystem>();
    }
}
