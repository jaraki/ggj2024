using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class PlayerShapes : MonoBehaviour {
    public const int GridSize = 4;
    public const int NumStates = 4;
    public GameObject CellPrefab;
    public Transform ModelParent;
    public int CurrentState;
    /// <summary>
    /// This has to be a 1d array in order for serialization to work correctly with the custom editor
    /// </summary>
    [SerializeField, HideInInspector]
    public int[] States = new int[GridSize * GridSize * NumStates];
    
    public int[] GetState(int index) {
        return SubArray(States, index, GridSize * GridSize);
    }

    public static T[] SubArray<T>(T[] array, int offset, int length) {
        T[] result = new T[length];
        Array.Copy(array, offset, result, 0, length);
        return result;
    }

    // Start is called before the first frame update
    void Start() {
        int[] currentState = GetState(CurrentState);
        for (int j = 0; j < GridSize; ++j) {
            for (int i = 0; i < GridSize; ++i) {
                if (currentState[j * GridSize + i] != 0) {
                    var go = Instantiate(CellPrefab, new Vector3(j, GridSize - i, 0), Quaternion.identity, ModelParent);
                    go.name = $"Cell ({j}, {i})";
                }
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
