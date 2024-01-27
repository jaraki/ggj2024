using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Level))]
public class LevelEditor : Editor {
    public override void OnInspectorGUI() {
        Level level = (Level)target;
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

        EditorGUILayout.BeginHorizontal(tableStyle);
        for (int x = -1; x < Level.Size; x++) {
            EditorGUILayout.BeginVertical((x == -1) ? headerColumnStyle : columnStyle);
            for (int y = -1; y < Level.Size; y++) {
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
                    level.Board[x * Level.Size + y] = EditorGUILayout.Toggle(level.Board[x * Level.Size + y] == 1) ? 1 : 0;
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.EndHorizontal();
    }
}