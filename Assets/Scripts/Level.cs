using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Level : MonoBehaviour {
    public string OpeningLine;
    public float TimeLimit = 15.0f;    
    public string[] EndingLines = new string[4];
    public string ClosingLine;
    public const int Size = 11;
    public GameObject CellPrefab;
    public FillShape FillShape;
    [HideInInspector]
    public int[] Board = new int[Size * Size];
    public Image[] fadeOutObjects;
    public AudioSource OpeningAudio;
    public AudioSource[] EndingAudio;
    public AudioSource ClosingAudio;
    public bool InvertedControls = false;
    public float passPercentage = 0.25f;

    public void Spawn() {
        for (int i = 0; i < Size; i++) {
            for (int j = 0; j < Size; j++) {
                if (Board[j * Size + i] != 0) {
                    var go = Instantiate(CellPrefab, new Vector3(j + 0.5f - Size / 2.0f, Size - i - 0.5f, 0), Quaternion.identity, transform);
                    go.name = $"Cell ({j}, {i})";
                }
            }
        }
        FillShape.Init();
    }

}
