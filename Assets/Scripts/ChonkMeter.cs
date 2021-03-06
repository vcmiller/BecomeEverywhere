﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChonkMeter : MonoBehaviour {
    public RectTransform levelsParent;
    public RectTransform meter;
    public Animator anim;
    public GameObject world;
    public Eater heEats;

    public MusicFade gameMusic;
    public float endDuration;

    private RectTransform[] levels;
    private CanvasGroup[] levelImages;
    private bool ended;

    // Start is called before the first frame update
    void Start() {
        levels = new RectTransform[levelsParent.childCount];
        levelImages = new CanvasGroup[levelsParent.childCount];
        for (int i = 0; i < levelsParent.childCount; i++) {
            levels[i] = levelsParent.GetChild(i) as RectTransform;
            levelImages[i] = levels[i].GetComponent<CanvasGroup>();

            if (i > heEats.curLevel) {
                levelImages[i].alpha = 0;
            }
        }
    }

    // Update is called once per frame
    void Update() {
        var p = meter.position;
        p.y = levels[heEats.curLevel].position.y;

        if (heEats.curLevel < levels.Length - 1) {
            float progress = heEats.GetNextLevelProgress();
            p.y = Mathf.Lerp(p.y, levels[heEats.curLevel + 1].position.y, progress);
        }
        meter.position = p;

        levelImages[heEats.curLevel].alpha = 1;

        if (!world && !ended) {
            anim.SetTrigger("GameOver");
            gameMusic.targetFadeLevel = 0;
            ended = true;
            Invoke("End", endDuration);
        }
    }

    private void End() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
