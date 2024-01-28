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

    IEnumerator SpawnDialog(string text) {
        var go = Instantiate(DialogPrefab, FindAnyObjectByType<Canvas>().transform);
        var dialog = go.GetComponent<Dialog>();
        yield return dialog.SetLine(text);
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
                    StartCoroutine(SpawnDialog(Levels[CurrentLevelIndex].OpeningLine));
                }
                
                currentCountdown -= Time.deltaTime;
                CountdownText.text = Math.Ceiling(currentCountdown).ToString();
                var delta = Math.Ceiling(currentCountdown) - currentCountdown;
                if (delta > 0) {
                    CountdownText.fontSize = (float)(originalFontSize / delta);
                }
            } else {
                if (State == GameState.Countdown) {
                    SpawnLevel();
                    State = GameState.Started;
                }

                var overlap = Mathf.RoundToInt(Levels[CurrentLevelIndex].FillShape.CalculateOverlap() * 100.0f);
                if (Timer.value > 0) {
                    FillText.text = $"{overlap}%";
                    Timer.value -= Time.deltaTime;
                }
                TimerText.text = Math.Ceiling(Timer.value).ToString();
                if (Timer.value <= 0 || overlap == 100) {
                    PlayerManager.SetFreeze(true);
                    if (State == GameState.Started) {
                        string closingLine = Levels[CurrentLevelIndex].ClosingLine;
                        if (overlap >= 0.76) {
                            StartCoroutine(StartNextLevel(Levels[CurrentLevelIndex].EndingLines[0], closingLine));
                        } else if (overlap >= 0.51) {
                            StartCoroutine(StartNextLevel(Levels[CurrentLevelIndex].EndingLines[1], closingLine));
                        } else if (overlap >= 0.26) {
                            StartCoroutine(StartNextLevel(Levels[CurrentLevelIndex].EndingLines[2], closingLine));
                        } else {
                            StartCoroutine(StartNextLevel(Levels[CurrentLevelIndex].EndingLines[3], closingLine));
                        }
                        
                    }
                    TimerText.text = "Time's Up!";
                    State = GameState.Ended;
                }
            }
        }

        IEnumerator StartNextLevel(string endingLine, string closingLine) {
            yield return StartCoroutine(SpawnDialog(endingLine));
            yield return StartCoroutine(SpawnDialog(closingLine));            
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
            PlayerManager.ResetPlayerSpawns();
        }
    }
}
