using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public float speed = 5.0f;
    public float jumpSpeed = 10f;

    //PlayerInput input;
    public Rigidbody body { get; private set; }
    public Collider[] colliders { get; private set; }
    RaycastHit[] hits = new RaycastHit[32];

    // Start is called before the first frame update
    void Awake() {
        body = GetComponent<Rigidbody>();
    }

    PlayerInput input;
    int layer;
    int groundLayers = 1 << Layers.Ground | 1 << Layers.Player1 | 1 << Layers.Player2 | 1 << Layers.Player3 | 1 << Layers.Player4;
    public void Init(PlayerInput input, int layer) {

        this.input = input;
        groundLayers &= ~(1 << layer); // remove your layer from the ground layers

        input.actions["Jump"].performed += Jump;
        input.actions["RotateRight"].performed += RotateRight;
        input.actions["RotateLeft"].performed += RotateLeft;

        colliders = GetComponentsInChildren<Collider>();

        // center player colliders around their center
        Vector3 averageOffset = Vector3.zero;
        int count = 0;
        foreach(var c in colliders) {
            averageOffset += c.transform.localPosition;
            count++;
        }
        if (count > 0) {
            averageOffset /= count;
            foreach (var c in colliders) {
                c.transform.localPosition = c.transform.localPosition - averageOffset;
            }
        }
    }

    Quaternion startRot = Quaternion.identity;
    Quaternion targetRot = Quaternion.identity;

    private void RotateRight(InputAction.CallbackContext obj) {
        startRot = body.rotation;
        targetRot *= Quaternion.Euler(0, 0, -90);
        time = 0.0f;
    }
    private void RotateLeft(InputAction.CallbackContext obj) {
        startRot = body.rotation;
        targetRot *= Quaternion.Euler(0, 0, 90);
        time = 0.0f;
    }

    float groundedLockout = 0.0f;
    void Jump(InputAction.CallbackContext obj) {
        if (grounded) {
            Vector3 vel = body.velocity;
            vel.y = jumpSpeed;
            body.velocity = vel;
        }
        grounded = false;
        groundedLockout = 0.1f;
    }

    float time = 0.0f;
    void Update() {
        if (!input) {
            return;
        }
        time += Time.deltaTime * 3.0f;
        Quaternion r = Quaternion.Lerp(startRot, targetRot, time);
        body.MoveRotation(r);
        //body.rotation = Quaternion.Euler(rot);
        groundedLockout -= Time.deltaTime;
        var move = input.actions["Move"].ReadValue<Vector2>();
        move *= speed;
        Vector3 vel = body.velocity;
        vel.x = move.x;
        vel.z = move.y;
        body.velocity = vel;

        grounded = false;

        foreach (var c in colliders) {
            int count = Physics.BoxCastNonAlloc(c.bounds.center, c.bounds.extents * .95f, Vector3.down, hits, Quaternion.identity, .1f, groundLayers);
            //Debug.DrawLine(c.bounds.center, c.bounds.center + Vector3.down);
            if (count > 0) {
                grounded = true;
                break;
            }
        }
    }


    bool grounded = false;

    private void OnTriggerExit(Collider other) {
        var shape = other.transform.parent.GetComponent<FillShape>();
        if (shape) {
            shape.RemovePlayer(this);
        }
    }

    private void OnTriggerStay(Collider other) {
        var shape = other.transform.parent.GetComponent<FillShape>();
        if (shape) {
            shape.AddPlayer(this);
        }
    }


}
