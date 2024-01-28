using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour {

    PlayerInputManager manager;

    public int NumPlayers = 0;

    public Transform[] spawns;
    List<PlayerController> players = new List<PlayerController>();

    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Awake() {
        manager = GetComponent<PlayerInputManager>();
        // make sure asset has "invoke c sharp events"
        manager.onPlayerJoined += Manager_onPlayerJoined;

        // need to manually join the players, was trying to do it only if they hit the join button but something was chalked there
        PlayerInput.Instantiate(playerPrefab, NumPlayers, "Keyboard", -1, Keyboard.current);
        PlayerInput.Instantiate(playerPrefab, NumPlayers, "Keyboard2", -1, Keyboard.current);
        PlayerInput.Instantiate(playerPrefab, NumPlayers, "Gamepad", -1, Gamepad.current);
        PlayerInput.Instantiate(playerPrefab, NumPlayers, "Gamepad2", -1, Gamepad.current);

    }

    //Dictionary<string, PlayerInput> players = new Dictionary<string, PlayerInput>();

    private void JoinAction_performed(InputAction.CallbackContext obj) {
        var binding = obj.action.GetBindingForControl(obj.control).Value;
        var scheme = binding.groups;
        //Debug.Log(binding.groups);


        //InputDevice device = obj.control.device is Keyboard ? Keyboard.current : Gamepad.current;
        //if(!players.ContainsKey(scheme)) {
        //    players[scheme] = PlayerInput.Instantiate(playerPrefab, NumPlayers, scheme, -1, device);
        //}

        //if (!players.ContainsKey(obj.control.ToString())) {

        //    if(obj.control.device is Keyboard) {
        //        PlayerInput.Instantiate(playerPrefab, NumPlayers, null, -1, Keyboard.current);
        //    }
        //    if (obj.control.device is Gamepad) {
        //        PlayerInput.Instantiate(playerPrefab, NumPlayers, null, -1, Gamepad.current);
        //    }
        //}
    }

    private void Manager_onPlayerJoined(PlayerInput input) {
        var controller = input.GetComponent<PlayerController>();
        var shape = input.GetComponent<PlayerShapes>();

        int layer = NumPlayers + 7;
        shape.Init(NumPlayers, layer);
        controller.Init(input, layer);
        SetSpawn(input.transform, NumPlayers);
        players.Add(controller);
        NumPlayers++;
    }

    void SetSpawn(Transform player, int index) {
        if (index >= 0 && index < spawns.Length) {
            player.position = spawns[index].position + Vector3.up * 3.0f;
        }
    }

    public void ResetPlayerSpawns() {
        for (int i = 0; i < players.Count; ++i) {
            SetSpawn(players[i].transform, i);
        }
    }

    public void SetFreeze(bool freeze) {
        foreach(var player in players) {
            player.body.isKinematic = freeze;
        }
    }

}
