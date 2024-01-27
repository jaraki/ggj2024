using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public float speed = 5.0f;
    public float jumpSpeed = 10f;

    //PlayerInput input;
    Rigidbody body;
    InputActions actions;


    // Start is called before the first frame update
    void Awake() {
        body = GetComponent<Rigidbody>();
        //input = GetComponent<PlayerInput>();
        // go click generate on the input actions asset then you get this nice class
        actions = new InputActions();
        actions.Enable();
        //actions.Game.Move.performed += MoveAction_performed;

        actions.Game.Jump.performed += Jump_performed;

    }

    private void Jump_performed(InputAction.CallbackContext obj) {
        Vector3 vel = body.velocity;
        vel.y = jumpSpeed;
        body.velocity = vel;
    }

    //private void MoveAction_performed(InputAction.CallbackContext obj) {
    //    //var curMove = obj.ReadValue<Vector2>();
    //    //Debug.Log(curMove);
    //}

    void Update() {
        var move = actions.Game.Move.ReadValue<Vector2>();
        move *= speed;
        Vector3 vel = body.velocity;
        vel.x = move.x;
        vel.z = move.y;
        body.velocity = vel;
    }


}
