using CGD.Case;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(WeaponElement))]
public class WeaponElementEditor : CaseElementEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        WeaponElement element = (WeaponElement)target;

        element.weapon = (Weapon)EditorGUILayout.ObjectField("Select Weapon: ", element.weapon, typeof(Weapon), false);
        EditorGUILayout.Space(10);

        if (element.weapon != null)
        {
            EditorGUILayout.LabelField("--------------------Weapon-------------------", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            if (element.weapon.icon)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(element.weapon.icon);

                GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));

                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
            }
            EditorGUILayout.EndVertical();


            EditorGUILayout.BeginVertical();
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.richText = true;


            EditorGUILayout.LabelField("<b>Name</b>             " +
                $"<b>{element.weapon.name}</b>", style);

            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }


        base.OnInspectorGUI();

        serializedObject.ApplyModifiedProperties();

    }
}
