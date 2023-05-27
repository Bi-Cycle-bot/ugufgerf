#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Door))]
public class DoorEditor : Editor
{
    public override void OnInspectorGUI() 
    {
        // Call normal GUI (displaying "a" and any other variables you might have)
        base.OnInspectorGUI();

        // Reference the variables in the script
        Door script = (Door)target;

        if (script.resetAtPoint) 
        {
            // Ensure the label and the value are on the same line
            EditorGUILayout.BeginHorizontal();

            // A label that says "b" (change b to B if you want it uppercase like default) and restricts its length.
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(50));
            // You can change 50 to any other value

            // Show and save the value of b
            script.endMinXY = EditorGUILayout.Vector2Field("endMinXY", script.endMinXY);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            // A label that says "b" (change b to B if you want it uppercase like default) and restricts its length.
            EditorGUILayout.LabelField("", GUILayout.MaxWidth(50));
            script.endMaxXY = EditorGUILayout.Vector2Field("endMaxXY", script.endMaxXY);

            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif