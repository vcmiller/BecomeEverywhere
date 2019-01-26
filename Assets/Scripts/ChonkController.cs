using SBR;
using UnityEngine;

public class ChonkController : PlayerController<CharacterChannels> {
    public float pitchMin = -80;
    public float pitchMax = 80;

    private Vector3 angles;

    protected override void DoInput() {
        channels.jump = false;
        base.DoInput();
    }

    public void Axis_Horizontal(float value) {
        Vector3 right = viewTarget.GetAlignedRight(transform.position);
        channels.movement += right * value;
    }

    public void Axis_Vertical(float value) {
        Vector3 fwd = viewTarget.GetAlignedForward(transform.position);
        channels.movement += fwd * value;
    }

    public void ButtonDown_Jump() {
        channels.jump = true;
    }

    public void Axis_MouseX(float value) {
        angles.y += value;

        channels.rotation = Quaternion.Euler(angles);
    }

    public void Axis_MouseY(float value) {
        angles.x -= value;

        if (angles.x < pitchMin) {
            angles.x = pitchMin;
        } else if (angles.x > pitchMax) {
            angles.x = pitchMax;
        }

        channels.rotation = Quaternion.Euler(angles);
    }
}
