using UnityEngine;

public class Edible : MonoBehaviour {
    private ParticleSystem ps;
    public float sizeRequired = 1;
    public float sizeGained = 1;
    public float particleDuration = 5;

    // Start is called before the first frame update
    void Start() {
        ps = GetComponentInChildren<ParticleSystem>();
    }

    public void Eat() {
        ps.transform.parent = null;
        ps.Play();
        Destroy(ps, particleDuration);
        Destroy(gameObject);
    }
}
