using CGD.Case;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Motive))]
public class MotiveEditor : CaseElementEditor
{
    private Dictionary<MotiveType, Sprite> spriteDict = new Dictionary<MotiveType, Sprite>();
    private void OnEnable()
    {
        foreach (MotiveType type in System.Enum.GetValues(typeof(MotiveType)))
        {
            string path = $"Sprites/Motives/{type}";
            Sprite sprite = Resources.Load<Sprite>(path);
            if (sprite != null)
            {
                spriteDict[type] = sprite;
            }
        }
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Motive motive = (Motive)target;

        EditorGUILayout.LabelField("--------------------Motive Settings-------------------", EditorStyles.centeredGreyMiniLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.BeginVertical();


        EditorGUILayout.LabelField("Motive Type", EditorStyles.boldLabel, GUILayout.Width(100));
        motive.type = (MotiveType)EditorGUILayout.EnumPopup(motive.type, GUILayout.Width(100));
        EditorGUILayout.EndVertical();
        motive.icon = GetSpriteForType(motive.type);

        if (motive.icon)
        {
            Texture2D texture = AssetPreview.GetAssetPreview(motive.icon);

            GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));

            GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
        }
        EditorGUILayout.EndHorizontal();


        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();
    }

    public Sprite GetSpriteForType(MotiveType type)
    {
        if (spriteDict.ContainsKey(type))
            return spriteDict[type];
        else
            return null;
    }
}
