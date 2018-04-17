using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(UFO))]
public class UFOEditor : Editor {
    public override void OnInspectorGUI() {
        var target = (UFO)(serializedObject.targetObject);
        target.UFOSpeed = EditorGUILayout.IntSlider("Speed", target.UFOSpeed, 0, 100);
        ProgressBar(target.UFOSpeed / 100.0f, "Speed");

        target.UFOColor = EditorGUILayout.ColorField("Color", target.UFOColor);
        
        //Blank Line
        EditorGUILayout.Space();
        target.startPos = EditorGUILayout.Vector3Field("StartPosition", target.startPos);
        
        //Blank Line
        EditorGUILayout.Space();
        target.startDirection = EditorGUILayout.Vector3Field("StartPosition", target.startDirection);
    }
    private void ProgressBar(float value, string label) {
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
        EditorGUILayout.Space();
    }
}