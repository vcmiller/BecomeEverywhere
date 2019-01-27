using SBR;
using System.Linq;
using UnityEngine;

public class ChonkMotor : Motor<CharacterChannels> {
    public Rigidbody rb { get; private set; }
    public Rigidbody[] rbs { get; private set; }
    public HingeJoint[] joints { get; private set; }
    public Eater eater { get; private set; }

    public SphereCollider sc { get; private set; }

    private (Vector3 anchor, Vector3 connectedAnchor, Quaternion rot)[] anchors;

    public bool grounded { get; private set; }
    public float groundHitTime { get; private set; }
    public RaycastHit groundHit => _groundHit;
    private RaycastHit _groundHit;
    private EdiblePlanet planet;

    private float mass;
    private float startBouncePitch;

    public float forceOffset = 1;
    public float acceleration;
    public float moveSpeed;
    public float jumpSpeed;
    public float groundCheck = 0.1f;
    public float gravity = 20;
    public float pitchDownPerLevel = 0.02f;
    public LayerMask ground = 1;
    public AudioParameters bounceSound;

    [Header("HE BIGG")]
    [SerializeField]
    private float _size = 1;
    public float size {
        get => _size;
        set {
            if (value != size) {
                float ratio = value / _size;
                transform.localScale *= ratio;
                for (int i = 0; i < joints.Length; i++) {
                    joints[i].transform.localRotation = anchors[i].rot;
                    joints[i].autoConfigureConnectedAnchor = false;
                    joints[i].anchor = anchors[i].anchor;
                    joints[i].connectedAnchor = anchors[i].connectedAnchor;
                }

                _size = value;

                bounceSound.pitch = startBouncePitch - eater.curLevel * pitchDownPerLevel;
            }
        }
    }
    
    protected override void Awake() {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        rbs = GetComponentsInChildren<Rigidbody>();
        joints = GetComponentsInChildren<HingeJoint>();
        eater = GetComponent<Eater>();
        anchors = joints.Select(j => (j.anchor, j.connectedAnchor, j.transform.localRotation)).ToArray();
        foreach (var r in rbs) {
            r.useGravity = false;
        }
        mass = rb.mass;
        startBouncePitch = bounceSound.pitch;
        planet = FindObjectOfType<EdiblePlanet>();
    }

    private void FixedUpdate() {
        if (planet) {
            float grav = Mathf.Sqrt(size);
            foreach (var r in rbs) {
                r.AddForce(-transform.position.normalized * gravity * grav, ForceMode.Acceleration);
            }
        }
    }

    private void Update() {
        UpdateGrounded();
    }

    protected override void DoOutput(CharacterChannels channels) {
        if (grounded) {
            Vector3 desiredVelocity = channels.movement * moveSpeed;
            Vector3 currentVelocity = Vector3.ProjectOnPlane(rb.velocity, transform.position);

            Vector3 dv = desiredVelocity - currentVelocity;
            float maxDv = acceleration * _size * Time.deltaTime / mass;
            if (dv.sqrMagnitude > maxDv * maxDv) {
                dv = dv.normalized * maxDv;
            }

            rb.AddForceAtPosition(dv, transform.position + transform.position.normalized * forceOffset * _size, ForceMode.VelocityChange);

            if (channels.jump) {
                Jump();
            }
        }
    }

    public void Jump() {
        rb.AddForce(transform.position.normalized * jumpSpeed * Mathf.Sqrt(size), ForceMode.Impulse);
    }

    private void UpdateGrounded() {
        bool wasGrounded = grounded;
        var up = transform.position.normalized;
        float radius = sc.radius * sc.transform.localScale.x;
        grounded = Physics.SphereCast(new Ray(transform.position + up * groundCheck * _size, -up), radius, out _groundHit, groundCheck * 2 * _size, ground);
        if (grounded && !wasGrounded) {
            groundHitTime = Time.time;
            bounceSound.PlayAtPoint(transform.position);
        }
    }
}
