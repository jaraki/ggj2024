using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour {
    public int RoundTime;
    public int CountdownTime;
    public Slider Timer;
    public Text TimerText;
    public Text CountdownText;
    private double currentCountdown;
    private int originalFontSize;
    // Start is called before the first frame update
    void Start() {
        ResetTimer();
        ResetCountdown();
        originalFontSize = CountdownText.fontSize;
    }

    void ResetTimer() {
        Timer.maxValue = RoundTime;
        Timer.value = RoundTime;
    }

    void ResetCountdown() {
        currentCountdown = CountdownTime;
    }

    // Update is called once per frame
    void Update() {
        if(currentCountdown > 0) {
            currentCountdown -= Time.deltaTime;
            CountdownText.text = Math.Ceiling(currentCountdown).ToString();
            CountdownText.fontSize = (int)(originalFontSize / (Math.Ceiling(currentCountdown) - currentCountdown));
        } else {
            if (Timer.value > 0) {
                Timer.value -= Time.deltaTime;
            }

            TimerText.text = Math.Ceiling(Timer.value).ToString();
            if (Timer.value == 0) {
                TimerText.text = "Time's Up!";
            }
        }

    }
}
