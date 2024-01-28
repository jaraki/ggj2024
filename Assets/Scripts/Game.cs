using System;
using System.Collections;
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
    public GameObject DialogPrefab;
    public int DialogDuration = 3;
    public AudioSource GameOverSound;
    public Level[] Levels;
    public string winningDialog = "Well done, now you can die anyways!";
    public const int MinPlayers = 4;
    public GameState State;
    public int CurrentLevelIndex;
    public PlayerManager PlayerManager;
    public int CountdownTime;
    public Slider Timer;
    public TMP_Text WaitingText;
    public TMP_Text TimerText;
    public TMP_Text CountdownText;
    public TMP_Text FillText;
    private double currentCountdown;
    private double originalFontSize;
    private float overlap;
    // Start is called before the first frame update
    void Start() {
        ResetTimer();
        ResetCountdown();
        originalFontSize = CountdownText.fontSize;
    }

    void SpawnLevel() {
        Levels[CurrentLevelIndex].Spawn();
    }

    void ResetTimer() {
        Timer.maxValue = Levels[CurrentLevelIndex].TimeLimit;
        Timer.value = Levels[CurrentLevelIndex].TimeLimit;
    }

    void ResetCountdown() {
        currentCountdown = CountdownTime;
    }

    void SpawnDialog(string text) {
        var go = Instantiate(DialogPrefab, FindAnyObjectByType<Canvas>().transform);
        var dialog = go.GetComponent<Dialog>();
        dialog.SetLine(text);
        Destroy(go, DialogDuration);
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
                if(State == GameState.Waiting) {
                    State = GameState.Countdown;
                    SpawnDialog(Levels[CurrentLevelIndex].OpeningLine);
                }
                
                currentCountdown -= Time.deltaTime;
                CountdownText.text = Math.Ceiling(currentCountdown).ToString();
                var delta = Math.Ceiling(currentCountdown) - currentCountdown;
                if (delta > 0) {
                    CountdownText.fontSize = (float)(originalFontSize / delta);
                }
            } else {
                if(State == GameState.Countdown) {
                    SpawnLevel();
                    State = GameState.Started;
                }
                if (Timer.value > 0) {
                    overlap = Levels[CurrentLevelIndex].FillShape.CalculateOverlap();
                    FillText.text = $"{overlap * 100.0f:0.0}%";
                    Timer.value -= Time.deltaTime;
                }

                TimerText.text = Math.Ceiling(Timer.value).ToString();
                if (Timer.value <= 0) {
                    if(State == GameState.Started) {
                        if (overlap >= 0.76) {
                            SpawnDialog(Levels[CurrentLevelIndex].EndingLines[0]);
                        } else if (overlap >= 0.51) {
                            SpawnDialog(Levels[CurrentLevelIndex].EndingLines[1]);
                        } else if (overlap >= 0.26) {
                            SpawnDialog(Levels[CurrentLevelIndex].EndingLines[2]);
                        } else {
                            SpawnDialog(Levels[CurrentLevelIndex].EndingLines[3]);
                        }
                        StartCoroutine(StartNextLevel());
                    }
                    TimerText.text = "Time's Up!";
                    State = GameState.Ended;
                }
            }
        }

        IEnumerator StartNextLevel() {
            yield return new WaitForSeconds(3);
            SpawnDialog(Levels[CurrentLevelIndex].ClosingLine);
            yield return new WaitForSeconds(3);

            State = GameState.Waiting;
            ResetTimer();
            ResetCountdown();
            Levels[CurrentLevelIndex].gameObject.SetActive(false);
            CurrentLevelIndex++;
            if (CurrentLevelIndex >= Levels.Length) {
                CurrentLevelIndex = 0;
                SpawnDialog(winningDialog);
                yield return new WaitForSeconds(3);
            }
            // toggle on anything
            foreach(var toggle in Levels[CurrentLevelIndex].toggleActive) {
                toggle.SetActive(!toggle.activeInHierarchy);
            }
        }

    }
}
