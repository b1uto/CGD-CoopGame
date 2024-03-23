using CGD.Case;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MotiveElement))]
public class MotiveElementEditor : CaseElementEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        MotiveElement element = (MotiveElement)target;

        element.motive = (Motive)EditorGUILayout.ObjectField("Select motive: ", element.motive, typeof(Motive), false);
        EditorGUILayout.Space(10);

        if (element.motive != null)
        {
            EditorGUILayout.LabelField("--------------------motive-------------------", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            if (element.motive.icon)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(element.motive.icon);

                GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));

                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical();
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;


            EditorGUILayout.LabelField("<b>Name</b>             " +
                $"<b>{element.motive.name}</b>", style);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }


        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();

    }
}