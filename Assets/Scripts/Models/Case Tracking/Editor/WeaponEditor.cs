using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

namespace CGD.Case
{
    [CustomEditor(typeof(Weapon))]
    public class WeaponEditor : Editor
    {
        private Sprite[] sprites;
        private int selectedSprite;

        private void OnEnable()
        {
            sprites = Resources.LoadAll<Sprite>("Sprites/Weapons");
            //foreach(Weapon.WeaponType type in System.Enum.GetValues(typeof(Weapon.WeaponType)))
            //{
            //    string path = $"Sprites/Weapons/{type}";
            //    Sprite sprite = Resources.Load<Sprite>(path);
            //    if (sprite != null)
            //    {
            //        spriteDict[type] = sprite;
            //    }
            //}
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Weapon weapon = (Weapon)target;

            EditorGUILayout.LabelField("--------------------Weapon Settings-------------------", EditorStyles.centeredGreyMiniLabel);



            weapon.type = (Weapon.WeaponType)EditorGUILayout.EnumPopup( "Weapon Type", weapon.type);
         

            if (sprites == null || sprites.Length == 0)
            {
                EditorGUILayout.HelpBox("There are no sprites at the file path", MessageType.Warning);
            }
            else
            {
                selectedSprite = Mathf.Max(0, System.Array.IndexOf(sprites, weapon.icon));


                //suspect.icon = (Sprite)EditorGUILayout.ObjectField(suspect.icon, typeof(Sprite), false);//, GUILayout.Width(100));
                selectedSprite = EditorGUILayout.Popup("Icon", selectedSprite, sprites.Select(x => x.name).ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    weapon.icon = sprites[selectedSprite];
                    weapon.name = sprites[selectedSprite].name;
                }
            }

            if (weapon.icon)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(weapon.icon);

                GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));

                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
            }  


            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }

        //public Sprite GetSpriteForType(Weapon.WeaponType type) 
        //{
        //    if(spriteDict.ContainsKey(type))
        //        return spriteDict[type];
        //    else 
        //        return null;
        //}
    }
}