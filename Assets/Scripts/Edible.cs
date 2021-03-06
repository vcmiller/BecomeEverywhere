﻿using UnityEngine;

public class Edible : MonoBehaviour {
    private ParticleSystem ps;
    public int levelRequired = 0;
    public float sizeGained = 1;
    public float particleDuration = 5;
    private Collider col;
    
    // Start is called before the first frame update
    void Start() {
        ps = GetComponentInChildren<ParticleSystem>();
        col = GetComponent<Collider>();
        Eater.LevelUp += UpdateLevel;
    }

    private void OnDestroy() {
        Eater.LevelUp -= UpdateLevel;
    }

    public void UpdateLevel(int level) {
        if (level >= levelRequired) {
            col.isTrigger = true;
        }
    }

    public virtual void Eat() {
        ps.transform.parent = null;
        ps.Play();
        Destroy(ps.gameObject, particleDuration);
        Destroy(gameObject);
    }
}
