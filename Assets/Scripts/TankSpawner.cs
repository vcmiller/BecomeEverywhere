using SBR;
using UnityEngine;

public class TankSpawner : MonoBehaviour {
    public int minLevel = 4;

    public float cooldown = 10;

    private CooldownTimer timer;
    private bool active;

    public GameObject tank;

    // Start is called before the first frame update
    void Start() {
        timer = new CooldownTimer(cooldown, Random.Range(0, cooldown));
        Eater.LevelUp += OnLevelUp;
    }

    private void OnDestroy() {
        Eater.LevelUp -= OnLevelUp;
    }

    public void OnLevelUp(int level) {
        if (level >= minLevel) {
            active = true;
        }
    }

    // Update is called once per frame
    void Update() {
        if (timer.Use() && active) {
            Instantiate(tank, transform.position, transform.rotation);
        }
    }
}
