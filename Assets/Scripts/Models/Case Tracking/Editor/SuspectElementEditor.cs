using CGD.Case;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SuspectElement))]
public class SuspectElementEditor : CaseElementEditor
{


    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        SuspectElement element = (SuspectElement)target;

        element.suspect = (Suspect)EditorGUILayout.ObjectField("Select Suspect: ", element.suspect, typeof(Suspect), false);
        EditorGUILayout.Space(10);

        if (element.suspect != null)
        {
            EditorGUILayout.LabelField("--------------------Suspect-------------------", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            if (element.suspect.icon)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(element.suspect.icon);

                GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));

                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
            }
            EditorGUILayout.EndVertical();

            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Personal Info", EditorStyles.boldLabel, GUILayout.Width(100));

            EditorGUILayout.Space(1);

            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;


            EditorGUILayout.LabelField("<b>Name</b>             " +
                $"<b>{element.suspect.title}</b> {element.suspect.fullName}", style);

            EditorGUILayout.LabelField("<b>Age</b>                " +
                $"{element.suspect.age}", style);
            
            EditorGUILayout.LabelField("<b>Eyes</b>              " +
                $"{element.suspect.eyeColour}", style);

            EditorGUILayout.LabelField("<b>Build</b>             " +
                $"<color=green>{element.suspect.height}</color>/{element.suspect.build}", style);

            

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

        }




        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }

}
