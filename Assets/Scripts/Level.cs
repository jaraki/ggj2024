using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level : MonoBehaviour {
    public const int Size = 11;
    public GameObject CellPrefab;
    [SerializeField]
    public int[] Board = new int[Size * Size];

    /// <summary>
    /// The percentage of how well the players matched the level layout
    /// </summary>
    /// <param name="blocks">player blocks</param>
    /// <returns>double from 0 to 1 representing the match percentage</returns>
    public double CalculatePercentage(int[,] blocks) {
        return 0;
    }

    // Start is called before the first frame update
    void Start() {
        for(int i = 0; i < Size; i++) {
            for(int j = 0;  j < Size; j++) {
                if (Board[i * Size + j] != 0) {
                    var go = Instantiate(CellPrefab, new Vector3(i, j, 0), Quaternion.identity, transform);
                    go.name = $"Cell ({i}, ${j})";
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
