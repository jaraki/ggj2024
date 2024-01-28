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

    public InputActionReference pauseAction;
    void Start() {
        currentTime = 0;
        startY = CreditsText.transform.position.y;
        pauseAction.action.performed += PauseAction;
        pauseAction.action.Enable();
    }

    private void PauseAction(InputAction.CallbackContext obj) {
        Loader.LoadMenu();
    }

    // Update is called once per frame
    void Update() {
        if (currentTime < CreditsTime) {
            currentTime += Time.deltaTime;
        }

        double percentage = currentTime / CreditsTime;
        if (percentage >= 1) {
            Loader.LoadMenu();
        }
        CreditsText.transform.position = new(CreditsText.transform.position.x, (float)((CreditsEndY - startY) * percentage), CreditsText.transform.position.z);
    }
}
