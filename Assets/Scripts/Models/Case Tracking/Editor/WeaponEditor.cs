using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;

namespace CGD.Case
{
    [CustomEditor(typeof(Weapon))]
    public class WeaponEditor : CaseElementEditor
    {
        private Dictionary<WeaponType, Sprite> spriteDict = new Dictionary<WeaponType, Sprite>();
        private void OnEnable()
        {
            foreach (WeaponType type in System.Enum.GetValues(typeof(WeaponType)))
            {
                string path = $"Sprites/{type}";
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

            Weapon weapon = (Weapon)target;

            EditorGUILayout.LabelField("--------------------Weapon Settings-------------------", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();


            EditorGUILayout.LabelField("Weapon Type", EditorStyles.boldLabel, GUILayout.Width(100));
            weapon.type = (WeaponType)EditorGUILayout.EnumPopup( weapon.type, GUILayout.Width(100));
            EditorGUILayout.EndVertical();
            weapon.icon = GetSpriteForType(weapon.type);

            if (weapon.icon)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(weapon.icon);

                GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));

                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
            }
            EditorGUILayout.EndHorizontal();    


            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }

        public Sprite GetSpriteForType(WeaponType type) 
        {
            if(spriteDict.ContainsKey(type))
                return spriteDict[type];
            else 
                return null;
        }
    }
}