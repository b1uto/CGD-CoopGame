using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;

namespace CGD.Case
{
    [CustomEditor(typeof(Suspect))]
    public class SuspectEditor : CaseElementEditor
    {
        private Sprite[] faceSprites;
        private int selectedSprite = 0;

        private string[] suspectNames = new string[]
        {
            "M: Aaron Armstrong", "F: Bella Brooks", "M: Charlie Chapman", "F: Daisy Dalton", 
            "M: Eddie Edwards", "F: Fiona Foster", "M: George Green", "F: Hannah Hunt", "M: Isaac Ingram", 
            "F: Jessica James", "M: Kevin King", "F: Lily Lewis", "M: Michael Morrison", "F: Nancy Nelson", 
            "M: Oliver Owens", "F: Patricia Peters", "M: Quentin Quinn", "F: Rachel Reed", "M: Steven Smith",
            "F: Tiffany Turner", "M: Ulysses Underwood", "F: Vanessa Vance", "M: William Wallace", "F: Xena Xavier", 
            "M: Yosef Young", "F: Zoe Zimmerman", "M: Adam Anderson", "F: Betty Brown", "M: Carl Carter", "F: Donna Davis",
            "M: Ethan Evans", "F: Felicity Ford", "M: Gary Gray", "F: Heidi Hamilton", "M: Ian Irwin", "F: Judy Jenkins",
            "M: Kyle Kent", "F: Laura Lane", "M: Max Mitchell", "F: Naomi Norris", "M: Oscar O'Donnell", 
            "F: Penny Parker", "M: Quincy Queen", "F: Ruby Russell", "M: Samuel Stone", "F: Tina Taylor", 
            "M: Uriah Upton", "F: Vivian Vincent", "M: Walter White", "F: Xandra Xylo", "M: Yanni York",
            "F: Zara Zane", "M: Alex Adams", "F: Barbara Baxter", "M: Cody Coleman", "F: Diana Doyle", "M: Evan Ellis",
            "F: Faith Fisher", "M: Greg Grant", "F: Holly Harper", "M: Ivan Ingram", "F: Janice Jefferson", 
            "M: Keith Knox", "F: Leah Lloyd", "M: Mason Moore", "F: Natalie Norman", "M: Owen Oliver", "F: Paige Phillips",
            "M: Quentin Quest", "F: Renee Roberts", "M: Shane Sherman", "F: Tracy Thomas", "M: Upton Ulrich",
            "F: Valerie Vaughn", "M: Wayne Watson", "F: Xyla Xander", "M: Yves Yates", "F: Zelda Zephyr", "M: Alan Archer",
            "F: Brooke Benson", "M: Chris Clarke", "F: Daphne Drake", "M: Elliot Edwards", "F: Francesca Freeman",
            "M: Gerald Gibson", "F: Ivy Irving", "M: Jack Jackson", "F: Kimberly Knox", "M: Lance Logan", "F: Monica Morris", 
            "M: Nathan Norris", "F: Olivia Owens", "M: Patrick Price", "F: Quinn Quigley", "M: Robert Robertson",
            "F: Sarah Simmons", "M: Thomas Tucker", "F: Uma Underhill", "M: Victor Vance", "F: Wendy Wilson", 
            "M: Xavier Xanadu", "F: Yolanda York", "M: Zachary Zimmerman"
        };


        private void OnEnable()
        {
            string path = $"Sprites/Suspects/";
            faceSprites = Resources.LoadAll<Sprite>(path);
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            Suspect suspect = (Suspect)target;

            EditorGUILayout.LabelField("--------------------Suspect Settings-------------------", EditorStyles.centeredGreyMiniLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(100));
            EditorGUILayout.LabelField("Select Icon", EditorStyles.boldLabel, GUILayout.Width(100));

            if (faceSprites == null || faceSprites.Length == 0)
            {
                EditorGUILayout.HelpBox("There are no sprites at the file path", MessageType.Warning);
            }
            else
            {
                selectedSprite = Mathf.Max(0, Array.IndexOf(faceSprites, suspect.icon));


                //suspect.icon = (Sprite)EditorGUILayout.ObjectField(suspect.icon, typeof(Sprite), false);//, GUILayout.Width(100));
                selectedSprite = EditorGUILayout.Popup(selectedSprite, faceSprites.Select(x => x.name).ToArray());
                if (EditorGUI.EndChangeCheck())
                {
                    suspect.icon = faceSprites[selectedSprite];
                }
            }
            if (suspect.icon)
            {
                Texture2D texture = AssetPreview.GetAssetPreview(suspect.icon);

                GUILayout.Label("", GUILayout.Height(100), GUILayout.Width(100));

                GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField("Personal Info", EditorStyles.boldLabel, GUILayout.Width(100));

            EditorGUILayout.Space(25);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Title", GUILayout.Width(80));
            suspect.title = (Title)EditorGUILayout.EnumPopup(suspect.title);
            EditorGUILayout.LabelField("Eye Colour", GUILayout.Width(80));
            suspect.eyeColour = (EyeColour)EditorGUILayout.EnumPopup(suspect.eyeColour);
            EditorGUILayout.LabelField("Height", GUILayout.Width(80));
            suspect.height = (Height)EditorGUILayout.EnumPopup(suspect.height);
            EditorGUILayout.LabelField("Build", GUILayout.Width(80));
            suspect.build = (Build)EditorGUILayout.EnumPopup(suspect.build);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Full Name", GUILayout.Width(80));
            suspect.fullName = EditorGUILayout.TextField(suspect.fullName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Age", GUILayout.Width(80));
            suspect.age = EditorGUILayout.IntField(suspect.age);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space(10);
            if (GUILayout.Button("Randomise Name"))
            {
                RandomiseName(suspect);
            }
            //EditorGUILayout.EndHorizontal();

          
            EditorGUILayout.EndVertical();

           

            
            EditorGUILayout.EndHorizontal();


            

            base.OnInspectorGUI();

            serializedObject.ApplyModifiedProperties();
        }



        private void RandomiseName(Suspect suspect) 
        {
            bool male = suspect.title == Title.Mr;
            var names = suspectNames.Where(x => x.Contains(male ? "M:": "F:")).ToArray();
           
            var newName = names[UnityEngine.Random.Range(0, names.Length)];

            suspect.fullName = newName.Split(':')[1];


        }
    }
}