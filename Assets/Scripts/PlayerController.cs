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

    void Update() {
        if (!input) {
            return;
        }
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
