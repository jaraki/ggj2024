using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum GameState {
    Waiting,
    Countdown,
    Started,
    Paused,
    Ended,
}

public class Game : MonoBehaviour {
    public TMP_Text InGameMenuTitle;
    public GameObject ResumeButton;
    public GameObject RestartButton;
    public GameObject InGameMenu;
    public GameObject DialogPrefab;
    public AudioSource GameOverSound;
    public AudioSource Music; // one at the end
    public AudioSource LoopMusic;
    public AudioSource CountdownSound;
    public AudioSource VictorySound;
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
    public Animator KingAnim;
    public EventSystem EventSystem;
    private GameState lastState = GameState.Started;

    // Start is called before the first frame update
    void Start() {
        originalFontSize = CountdownText.fontSize;
        StartCoroutine(Countdown());
        Timer.gameObject.SetActive(false);
        InGameMenu.SetActive(false);

        InputActions act = new();
        act.Game.Enable();
        act.Game.Pause.performed += PauseAction;
    }

    void SpawnLevel() {
        Levels[CurrentLevelIndex].Spawn();
    }

    IEnumerator SpawnDialog(string text, float duration) {
        KingAnim.SetBool("isTalking", true);
        var go = Instantiate(DialogPrefab, FindAnyObjectByType<Canvas>().transform);
        var dialog = go.GetComponent<Dialog>();
        yield return dialog.SetLine(text, duration);
        KingAnim.SetBool("isTalking", false);
    }

    public void Resume() {
        Time.timeScale = 1f;
        State = lastState;
        InGameMenu.SetActive(false);
    }

    private void PauseAction(InputAction.CallbackContext obj) {
        if (State == GameState.Paused) {
            Resume();
        } else {
            lastState = State;
            State = GameState.Paused;
            Time.timeScale = 0f;
            InGameMenu.SetActive(true);
            ResumeButton.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update() {
        if (PlayerManager.NumPlayers < MinPlayers) {
            int difference = MinPlayers - PlayerManager.NumPlayers;
            WaitingText.text = $"Waiting for {difference} More Players...";
            State = GameState.Waiting;
        } else if (State == GameState.Started) {
            WaitingText.text = "";
            if (LoopMusic && !LoopMusic.isPlaying) {
                LoopMusic.Play();
            }
            var overlap = Mathf.RoundToInt(Levels[CurrentLevelIndex].FillShape.CalculateOverlap() * 100.0f);
            if (overlap >= 99) {
                overlap = 100;
            }
            if (Timer.value > 0) {
                FillText.text = $"{overlap}%";
                Timer.value -= Time.deltaTime;
            }
            if (Timer.value <= Music.clip.length) {
                //if (LoopMusic && LoopMusic.isPlaying) {
                //    LoopMusic.Stop();
                //}
                //if (Music && !Music.isPlaying) {
                //    Music.Play();
                //}
            }
            TimerText.text = Math.Ceiling(Timer.value).ToString();
            if (Timer.value <= 0 || overlap == 100) {
                //if (Music && Music.isPlaying) {
                //    Music.Stop();
                //}
                if (LoopMusic && LoopMusic.isPlaying) {
                    LoopMusic.Stop();
                }
                PlayerManager.SetFreeze(true);
                if (State == GameState.Started) {
                    var level = Levels[CurrentLevelIndex];
                    string closingLine = level.ClosingLine;
                    int index = 0;
                    if(overlap < level.passPercentage) {
                        index = 3;
                        StartCoroutine(GameOver(index));
                    } else {
                        if (overlap >= 75) {
                            index = 0;
                        } else if (overlap >= 50) {
                            index = 1;
                            KingAnim.SetTrigger("Dissaproval");
                        } else if (overlap >= 25) {
                            index = 2;
                            KingAnim.SetTrigger("Sad");
                        } else {
                            index = 3;
                            KingAnim.SetTrigger("Sad");
                        }
                        if (index < 3) {
                            StartCoroutine(StartNextLevel(index, closingLine));
                        }
                    }
                }
                TimerText.text = "Time's Up!";
                State = GameState.Ended;
            }
        }
    }

    IEnumerator StartNextLevel(int endingLineIndex, string closingLine) {
        var level = Levels[CurrentLevelIndex];
        level.EndingAudio[endingLineIndex].Play();
        yield return StartCoroutine(SpawnDialog(level.EndingLines[endingLineIndex], level.EndingAudio[endingLineIndex].clip.length));
        level.ClosingAudio.Play();
        yield return StartCoroutine(SpawnDialog(closingLine, level.ClosingAudio.clip.length));
        State = GameState.Waiting;
        level.gameObject.SetActive(false);
        CurrentLevelIndex++;
        if (CurrentLevelIndex >= Levels.Length) {
            yield return StartCoroutine(WinGame());
        } else {
            level = Levels[CurrentLevelIndex];
            foreach (var img in level.fadeOutObjects) {
                img.gameObject.SetActive(true);
            }
            PlayerManager.SetFreeze(false);
            PlayerManager.ResetPlayerSpawns();
            StartCoroutine(Countdown());
        }
    }

    IEnumerator WinGame() {
        float duration = 3;
        yield return StartCoroutine(SpawnDialog(winningDialog, duration));
        yield return new WaitForSeconds(duration);
        if (VictorySound && !VictorySound.isPlaying) {
            VictorySound.Play();
        }
        // TODO: winning cutscene
        InGameMenu.SetActive(true);
        InGameMenuTitle.text = "You Win!";
        ResumeButton.SetActive(false);
    }

    IEnumerator GameOver(int index) {
        var level = Levels[CurrentLevelIndex];
        string endingLine = level.EndingLines[index];
        string closingLine = level.ClosingLine;
        level.EndingAudio[index].Play();
        float endingDuration = level.EndingAudio[index].clip.length;
        yield return StartCoroutine(SpawnDialog(endingLine, endingDuration));
        level.ClosingAudio.Play();
        float closingDuration = level.ClosingAudio.clip.length;
        yield return StartCoroutine(SpawnDialog(closingLine, closingDuration));
        yield return new WaitForSeconds(3);
        if (GameOverSound && !GameOverSound.isPlaying) {
            GameOverSound.Play();
        }
        // TODO: winning cutscene
        InGameMenu.SetActive(true);
        InGameMenuTitle.text = "Game Over!";
        ResumeButton.SetActive(false);
        EventSystem.SetSelectedGameObject(RestartButton);
    }

    IEnumerator Countdown() {
        var level = Levels[CurrentLevelIndex];
        PlayerController.InvertedControls = level.InvertedControls;
        Timer.maxValue = level.TimeLimit;
        Timer.value = level.TimeLimit;
        if (level.OpeningAudio) {
            level.OpeningAudio.Play();
            yield return StartCoroutine(SpawnDialog(level.OpeningLine, level.OpeningAudio.clip.length));
        }
        float timer = CountdownTime;
        if (CountdownSound && !CountdownSound.isPlaying) {
            CountdownSound.Play();
        }
        while (timer > 0) {
            CountdownText.text = Math.Ceiling(timer).ToString();
            var delta = Math.Ceiling(timer) - timer;
            if (delta < 0.25f) {
                delta = 0.25f;
            }
            foreach (var img in level.fadeOutObjects) {
                img.color = new Color(1, 1, 1, timer / (CountdownTime / 2.0f));
            }
            CountdownText.fontSize = (float)(originalFontSize / delta);
            timer -= Time.deltaTime * 1.5f;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        foreach (var img in level.fadeOutObjects) {
            img.gameObject.SetActive(false);
        }
        CountdownText.text = "";
        SpawnLevel();
        Timer.gameObject.SetActive(true);
        State = GameState.Started;
    }
}