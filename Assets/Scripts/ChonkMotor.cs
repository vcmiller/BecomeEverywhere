using SBR;
using UnityEngine;

public class ChonkMotor : Motor<CharacterChannels> {
    public Rigidbody rb { get; private set; }
    public Rigidbody[] rbs { get; private set; }
    public SphereCollider sc { get; private set; }

    public bool grounded { get; private set; }
    public float groundHitTime { get; private set; }
    public RaycastHit groundHit => _groundHit;
    private RaycastHit _groundHit;

    public Vector3 forceOffset = Vector3.up;
    public float acceleration;
    public float moveSpeed;
    public float jumpSpeed;
    public float groundCheck = 0.1f;
    public float gravity = 20;

    public LayerMask ground = 1;

    protected override void Awake() {
        base.Awake();
        rb = GetComponent<Rigidbody>();
        sc = GetComponent<SphereCollider>();
        rbs = GetComponentsInChildren<Rigidbody>();
        foreach (var r in rbs) {
            r.useGravity = false;
        }
    }

    private void FixedUpdate() {
        foreach (var r in rbs) {
            r.AddForce(-transform.position.normalized * gravity, ForceMode.Acceleration);
        }
    }

    private void Update() {
        UpdateGrounded();

        if (Input.GetButtonDown("Fire1")) {
            transform.localScale *= 1.2f;
        }
    }

    protected override void DoOutput(CharacterChannels channels) {
        if (grounded) {
            Vector3 desiredVelocity = channels.movement * moveSpeed;
            Vector3 currentVelocity = rb.velocity;
            currentVelocity.y = 0;

            Vector3 dv = desiredVelocity - currentVelocity;
            float maxDv = acceleration * Time.deltaTime / rb.mass;
            if (dv.sqrMagnitude > maxDv * maxDv) {
                dv = dv.normalized * maxDv;
            }

            rb.AddForceAtPosition(dv, transform.position + forceOffset, ForceMode.VelocityChange);

            if (channels.jump) {
                rb.AddForce(transform.position.normalized * jumpSpeed, ForceMode.Impulse);
            }
        }
    }

    private void UpdateGrounded() {
        bool wasGrounded = grounded;
        var up = transform.position.normalized;
        float radius = sc.radius * sc.transform.localScale.x;
        grounded = Physics.SphereCast(new Ray(transform.position + up * groundCheck, -up), radius, out _groundHit, groundCheck * 2, ground);
        if (grounded && !wasGrounded) {
            groundHitTime = Time.time;
        }
    }
}
