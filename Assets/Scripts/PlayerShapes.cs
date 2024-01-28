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
        Array.Copy(array, offset * length, result, 0, length);
        return result;
    }

    // Start is called before the first frame update
    void Start() {

    }

    public void Init(int mode, int layer, Material mat) {
        CurrentState = mode;
        int[] currentState = GetState(CurrentState);
        for (int j = 0; j < GridSize; ++j) {
            for (int i = 0; i < GridSize; ++i) {
                if (currentState[j * GridSize + i] != 0) {
                    var go = Instantiate(CellPrefab, Vector3.zero, Quaternion.identity, ModelParent);
                    go.layer = layer;
                    go.transform.localPosition = new Vector3(j + 0.5f, GridSize - i - 0.5f, 0);
                    go.name = $"Cell ({j}, {i})";
                }
            }
        }
        var rends = GetComponentsInChildren<Renderer>();
        foreach(var rend in rends) {
            rend.material = mat;
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
