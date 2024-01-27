using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public float speed = 5.0f;

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
    }

    //private void MoveAction_performed(InputAction.CallbackContext obj) {
    //    //var curMove = obj.ReadValue<Vector2>();
    //    //Debug.Log(curMove);
    //}

    void Update() {
        var move = actions.Game.Move.ReadValue<Vector2>();
        move *= speed;
        body.velocity = new Vector3(move.x, 0.0f, move.y);
    }


}
