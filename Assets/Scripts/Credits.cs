using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Credits : MonoBehaviour {
    public Loader Loader;
    public TMP_Text CreditsText;
    public int CreditsTime;
    public int CreditsEndY;
    private double currentTime;
    private double startY;
    // Start is called before the first frame update

    void Start() {
        currentTime = 0;
        startY = CreditsText.transform.position.y;
        InputActions act = new InputActions();
        act.Game.Enable();
        act.Game.Pause.performed += PauseAction;
    }

    private void PauseAction(InputAction.CallbackContext obj) {
        Loader.LoadMenu();
    }

    // Update is called once per frame
    void Update() {
        if (currentTime < CreditsTime) {
            currentTime += Time.deltaTime * 1f;
        }

        double percentage = currentTime / CreditsTime;
        if (percentage >= 1) {
            Loader.LoadMenu();
        }
        CreditsText.transform.position = new(CreditsText.transform.position.x, (float)((CreditsEndY - startY) * percentage), CreditsText.transform.position.z);
    }
}
