using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{

    PlayerInputManager manager;

    public int NumPlayers = 0;

    // Start is called before the first frame update
    void Awake()
    {
        manager = GetComponent<PlayerInputManager>();
        // make sure asset has "invoke c sharp events"
        manager.onPlayerJoined += Manager_onPlayerJoined;
    }

    private void Manager_onPlayerJoined(PlayerInput input) {
        var controller = input.GetComponent<PlayerController>();
        var shape = input.GetComponent<PlayerShapes>();
        shape.Init(NumPlayers++);
        controller.Init(input);
        input.transform.position = Vector3.up * 3.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
