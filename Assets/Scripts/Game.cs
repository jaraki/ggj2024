using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum GameState {
    Waiting,
    Countdown,
    Started,
    Ended,
}

public class Game : MonoBehaviour {
    public const int MinPlayers = 4;
    public GameState State;
    public PlayerManager PlayerManager;
    public int RoundTime;
    public int CountdownTime;
    public Slider Timer;
    public TMP_Text WaitingText;
    public TMP_Text TimerText;
    public TMP_Text CountdownText;
    private double currentCountdown;
    private double originalFontSize;
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
        if(PlayerManager.NumPlayers < MinPlayers) {
            int difference = MinPlayers - PlayerManager.NumPlayers;
            WaitingText.text = $"Waiting for {difference} More Players...";
            State = GameState.Waiting;
        } else {
            WaitingText.text = "";
            if (currentCountdown > 0) {
                State = GameState.Countdown;
                currentCountdown -= Time.deltaTime;
                CountdownText.text = Math.Ceiling(currentCountdown).ToString();
                var delta = Math.Ceiling(currentCountdown) - currentCountdown;
                if (delta > 0) {
                    CountdownText.fontSize = (float)(originalFontSize / delta);
                }
            } else {
                if (Timer.value > 0) {
                    State = GameState.Started;
                    Timer.value -= Time.deltaTime;
                }

                TimerText.text = Math.Ceiling(Timer.value).ToString();
                if (Timer.value == 0) {
                    TimerText.text = "Time's Up!";
                    State = GameState.Ended;
                }
            }
        }


    }
}
