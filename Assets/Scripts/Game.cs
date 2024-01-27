using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    public int RoundTime;
    public Slider Timer;
    public Text TimerText;
    // Start is called before the first frame update
    void Start() {
        Timer.maxValue = RoundTime;
        Timer.value = RoundTime;
    }

    // Update is called once per frame
    void Update() {
        Timer.value -= Time.deltaTime;
        TimerText.text = Math.Ceiling(Timer.value).ToString();
        if(Timer.value == 0) {
            TimerText.text = "Time's Up!";
        }
    }
}
