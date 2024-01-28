using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level : MonoBehaviour {
    public string OpeningLine;
    public string[] EndingLines = new string[5];
    public const int Size = 11;
    public GameObject CellPrefab;
    [HideInInspector]
    public int[] Board = new int[Size * Size];

    // Start is called before the first frame update
    void Awake() {
        for (int i = 0; i < Size; i++) {
            for (int j = 0; j < Size; j++) {
                if (Board[j * Size + i] != 0) {
                    var go = Instantiate(CellPrefab, new Vector3(j + 0.5f - Size / 2.0f, Size - i - 0.5f, 0), Quaternion.identity, transform);
                    go.name = $"Cell ({j}, {i})";
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
