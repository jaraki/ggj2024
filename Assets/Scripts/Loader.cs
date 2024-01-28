using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void LoadMain() {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1.0f;
    }

    public void LoadCredits() {
        SceneManager.LoadScene("Credits");
        Time.timeScale = 1.0f;
    }

    public void LoadMenu() {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1.0f;
    }

    public void Quit() {
        Application.Quit();
    }
}
