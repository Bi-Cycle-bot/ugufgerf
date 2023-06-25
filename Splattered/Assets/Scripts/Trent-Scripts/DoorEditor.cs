// Trent Lucas
// 25 June 2023

// Door editor helper
// Displays end zone parameters when resetAtPoint is enabled
////////////////////////////////////////////////////////////////

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
        base.OnInspectorGUI();

        // Reference the variables in the script
        Door script = (Door)target;

        if (script.resetAtPoint) 
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("", GUILayout.MaxWidth(50));

            script.endMinXY = EditorGUILayout.Vector2Field("endMinXY", script.endMinXY);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("", GUILayout.MaxWidth(50));
            script.endMaxXY = EditorGUILayout.Vector2Field("endMaxXY", script.endMaxXY);

            EditorGUILayout.EndHorizontal();
        }
    }
}
#endif