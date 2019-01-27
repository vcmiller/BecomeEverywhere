using UnityEngine;
using UnityEngine.UI;

public class MenuActions : MonoBehaviour {
    private Animator anim;
    
    public Selectable startSelected;
    public Selectable infoSelected;

    public GameObject player;
    public GameObject gameCanvas;

    public MusicFade mainMusic;
    public MusicFade menuMusic;


    private void Start() {
        anim = GetComponent<Animator>();
    }

    public void Play() {
        anim.SetTrigger("Play");
    }

    public void BackFromInfo() {
        anim.SetBool("Info", false);
        startSelected.Select();
    }

    public void Info() {
        anim.SetBool("Info", true);
        infoSelected.Select();
    }

    public void Quit() {
        Application.Quit();
    }

    public void ReadyToPlay() {
        player.SetActive(true);
        gameCanvas.SetActive(true);
        gameObject.SetActive(false);

        menuMusic.targetFadeLevel = 0;
        mainMusic.targetFadeLevel = 1;
    }
}
