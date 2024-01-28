using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            Loader.LoadMenu();
        }
        if(currentTime < CreditsTime) {
            currentTime += Time.deltaTime;
        }

        double percentage = currentTime / CreditsTime;
        if(percentage >= 1) {
            Loader.LoadMenu();
        }
        CreditsText.transform.position = new(CreditsText.transform.position.x, (float)((CreditsEndY - startY) * percentage), CreditsText.transform.position.z);
    }
}
