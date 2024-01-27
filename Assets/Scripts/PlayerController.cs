using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public float speed = 5.0f;
    public float jumpSpeed = 10f;

    //PlayerInput input;
    Rigidbody body;
    public Collider[] colliders { get; private set; }

    // Start is called before the first frame update
    void Awake() {
        body = GetComponent<Rigidbody>();
        colliders = GetComponentsInChildren<Collider>();
    }

    PlayerInput input;
    public void Init(PlayerInput input) {
        this.input = input;

        input.actions["Jump"].performed += Jump;
        input.actions["RotateRight"].performed += RotateRight;
        input.actions["RotateLeft"].performed += RotateLeft;
    }

    float targetZRot = 0.0f;

    Quaternion startRot = Quaternion.identity;
    Quaternion targetRot = Quaternion.identity;

    private void RotateRight(InputAction.CallbackContext obj) {
        startRot = body.rotation;
        targetZRot -= 90;
        targetRot *= Quaternion.Euler(0, 0, -90);
        time = 0.0f;
    }
    private void RotateLeft(InputAction.CallbackContext obj) {
        startRot = body.rotation;
        targetZRot += 90;
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
        time += Time.deltaTime * 2.0f;
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
    }

    private void OnTriggerEnter(Collider other) {

    }

    bool grounded = false;
    private void OnCollisionStay(Collision collision) {
        if (groundedLockout < 0.0f) {
            grounded = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        var shape = other.transform.root.GetComponent<FillShape>();
        if (shape) {
            shape.RemovePlayer(this);
        }
    }

    private void OnTriggerStay(Collider other) {
        var shape = other.transform.root.GetComponent<FillShape>();
        if (shape) {
            shape.AddPlayer(this);
        }
    }


}
