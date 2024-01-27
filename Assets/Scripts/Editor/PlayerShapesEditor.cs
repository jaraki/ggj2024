using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerShapes))]
public class PlayerShapesEditor : Editor {
    public override void OnInspectorGUI() {
        PlayerShapes playerShapes = (PlayerShapes)target;
        EditorGUILayout.Space();
        DrawDefaultInspector();
        GUIStyle tableStyle = new("box") {
            padding = new RectOffset(10, 10, 10, 10)
        };
        tableStyle.margin.left = 32;

        GUIStyle headerColumnStyle = new() {
            fixedWidth = 35
        };

        GUIStyle columnStyle = new() {
            fixedWidth = 20
        };

        GUIStyle rowStyle = new() {
            fixedHeight = 20
        };

        GUIStyle rowHeaderStyle = new() {
            fixedWidth = columnStyle.fixedWidth - 1
        };

        GUIStyle columnHeaderStyle = new() {
            fixedWidth = 30,
            fixedHeight = 20
        };

        GUIStyle columnLabelStyle = new() {
            fixedWidth = rowHeaderStyle.fixedWidth - 6,
            alignment = TextAnchor.MiddleCenter,
            fontStyle = FontStyle.Bold
        };

        GUIStyle cornerLabelStyle = new() {
            fixedWidth = 25,
            alignment = TextAnchor.MiddleRight,
            fontStyle = FontStyle.BoldAndItalic,
            fontSize = 14
        };
        cornerLabelStyle.padding.top = -5;

        GUIStyle rowLabelStyle = new() {
            fixedWidth = 25,
            alignment = TextAnchor.MiddleRight,
            fontStyle = FontStyle.Bold
        };

        for(int i = 0; i < PlayerShapes.NumStates; i++) {
            EditorGUILayout.BeginHorizontal(tableStyle);
            for (int x = -1; x < PlayerShapes.GridSize; x++) {
                EditorGUILayout.BeginVertical((x == -1) ? headerColumnStyle : columnStyle);
                for (int y = -1; y < PlayerShapes.GridSize; y++) {
                    if (x == -1 && y == -1) {
                        EditorGUILayout.BeginVertical(rowHeaderStyle);
                        EditorGUILayout.LabelField("[X,Y]", cornerLabelStyle);
                        EditorGUILayout.EndHorizontal();
                    } else if (x == -1) {
                        EditorGUILayout.BeginVertical(columnHeaderStyle);
                        EditorGUILayout.LabelField(y.ToString(), rowLabelStyle);
                        EditorGUILayout.EndHorizontal();
                    } else if (y == -1) {
                        EditorGUILayout.BeginVertical(rowHeaderStyle);
                        EditorGUILayout.LabelField(x.ToString(), columnLabelStyle);
                        EditorGUILayout.EndHorizontal();
                    }

                    if (x >= 0 && y >= 0) {
                        EditorGUILayout.BeginHorizontal(rowStyle);
                        playerShapes.States[i * PlayerShapes.GridSize * PlayerShapes.GridSize + x * PlayerShapes.GridSize + y] = EditorGUILayout.Toggle(playerShapes.States[i * PlayerShapes.GridSize * PlayerShapes.GridSize + x * PlayerShapes.GridSize + y] == 1) ? 1 : 0;
                        EditorGUILayout.EndHorizontal();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();
        }

    }
}
