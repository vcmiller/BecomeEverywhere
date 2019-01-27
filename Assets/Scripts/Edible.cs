using UnityEngine;

public class Edible : MonoBehaviour {
    private ParticleSystem ps;
    public int levelRequired = 0;
    public float sizeGained = 1;
    public float particleDuration = 5;
    
    // Start is called before the first frame update
    void Start() {
        ps = GetComponentInChildren<ParticleSystem>();
    }

    public virtual void Eat() {
        ps.transform.parent = null;
        ps.Play();
        Destroy(ps.gameObject, particleDuration);
        Destroy(gameObject);
    }
}
