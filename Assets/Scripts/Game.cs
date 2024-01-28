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
    private double originalFontSize;

    // Start is called before the first frame update
    void Start() {
        originalFontSize = CountdownText.fontSize;
        StartCoroutine(Countdown());
    }

    void SpawnLevel() {
        Levels[CurrentLevelIndex].Spawn();
    }


    IEnumerator SpawnDialog(string text) {
        var go = Instantiate(DialogPrefab, FindAnyObjectByType<Canvas>().transform);
        var dialog = go.GetComponent<Dialog>();
        yield return dialog.SetLine(text);
    }

    // Update is called once per frame
    void Update() {
        if (PlayerManager.NumPlayers < MinPlayers) {
            int difference = MinPlayers - PlayerManager.NumPlayers;
            WaitingText.text = $"Waiting for {difference} More Players...";
            State = GameState.Waiting;
        } else if (State == GameState.Started) {
            WaitingText.text = "";

            var overlap = Mathf.RoundToInt(Levels[CurrentLevelIndex].FillShape.CalculateOverlap() * 100.0f);
            if (overlap >= 99) {
                overlap = 100;
            }
            if (Timer.value > 0) {
                FillText.text = $"{overlap}%";
                Timer.value -= Time.deltaTime;
            }
            TimerText.text = Math.Ceiling(Timer.value).ToString();
            if (Timer.value <= 0 || overlap == 100) {
                Timer.value = 0;
                PlayerManager.SetFreeze(true);
                if (State == GameState.Started) {
                    string closingLine = Levels[CurrentLevelIndex].ClosingLine;
                    int line;
                    if (overlap >= 0.76) {
                        line = 0;
                    } else if (overlap >= 0.51) {
                        line = 1;
                    } else if (overlap >= 0.26) {
                        line = 2;
                    } else {
                        line = 3;
                    }
                    StartCoroutine(StartNextLevel(Levels[CurrentLevelIndex].EndingLines[line], closingLine));
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
        Levels[CurrentLevelIndex].gameObject.SetActive(false);
        CurrentLevelIndex++;
        if (CurrentLevelIndex >= Levels.Length) {
            CurrentLevelIndex = 0;
            yield return StartCoroutine(SpawnDialog(winningDialog));
            // todo: load winning scene
        }
        // toggle on anything
        foreach (var toggle in Levels[CurrentLevelIndex].toggleActive) {
            toggle.SetActive(!toggle.activeInHierarchy);
        }
        PlayerManager.SetFreeze(false);
        PlayerManager.ResetPlayerSpawns();
        StartCoroutine(Countdown());
    }

    IEnumerator Countdown() {
        Timer.maxValue = Levels[CurrentLevelIndex].TimeLimit;
        Timer.value = Levels[CurrentLevelIndex].TimeLimit;
        yield return StartCoroutine(SpawnDialog(Levels[CurrentLevelIndex].OpeningLine));
        float timer = CountdownTime;
        while (timer > 0) {
            CountdownText.text = Math.Ceiling(timer).ToString();
            var delta = Math.Ceiling(timer) - timer;
            if(delta < 0.01f) {
                delta = 0.01f;
            }
            CountdownText.fontSize = (float)(originalFontSize / delta);
            timer -= Time.deltaTime * 2.0f;
            yield return null;
        }
        CountdownText.text = "";
        SpawnLevel();
        State = GameState.Started;
    }
}