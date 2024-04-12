using CGD.Economy;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScoreSettings))]
public class ScoreSettingsEditor : Editor
{
    private ScoreSettings scoreSettings;
    private List<int> scoreValues = new List<int>();
    System.Array enumValues = System.Enum.GetValues(typeof(ScoreType));

    private SerializedProperty scoreValuesProperty;

    private void OnEnable()
    {
        // Cache the SerializedProperty
        scoreValuesProperty = serializedObject.FindProperty("scoreValues");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); 

        EditorGUI.BeginChangeCheck(); 

        ScoreType[] enumValues = (ScoreType[])System.Enum.GetValues(typeof(ScoreType));
        for (int i = 0; i < enumValues.Length; i++)
        {
            if (i >= scoreValuesProperty.arraySize)
            {
                scoreValuesProperty.arraySize++;
                serializedObject.ApplyModifiedProperties();
            }

            SerializedProperty scoreValue = scoreValuesProperty.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(scoreValue, new GUIContent(enumValues[i].ToString()), true);

        }

        if (EditorGUI.EndChangeCheck())
        {
            serializedObject.ApplyModifiedProperties(); 
        }

        EditorGUI.EndChangeCheck();
    }

}
