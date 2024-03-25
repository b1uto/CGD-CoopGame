using CGD.Case;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[ExecuteInEditMode]
public class debugsuspectcreator : MonoBehaviour
{
#if UNITY_EDITOR
    public InspectorButton create = new InspectorButton("CreateSuspects");
    public InspectorButton creatmot = new InspectorButton("CreateMotives");
#endif

    //private void CreateSuspects()
    //{
    //    var faceSprites = Resources.LoadAll<Sprite>($"Sprites/Suspects/");


    //    System.IO.Directory.CreateDirectory($"Assets/Data/Suspects");
    //    foreach (var sprite in faceSprites)
    //    {
    //        var suspect = ScriptableObject.CreateInstance<Suspect>();
    //        suspect.icon = sprite;
    //        suspect.fullName = sprite.name;
    //        suspect.name = sprite.name;

    //        var path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Data/Suspects/{sprite.name}.asset");

    //        //AssetDatabase.AddObjectToAsset(newClue, caseElement);
    //        AssetDatabase.CreateAsset(suspect, path);
    //    }


    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //}

    //private void CreateMotives()
    //{
    //    var faceSprites = Resources.LoadAll<Sprite>($"Sprites/Motives/");


    //    System.IO.Directory.CreateDirectory($"Assets/Data/Motives");
    //    foreach (var sprite in faceSprites)
    //    {
    //        var motive = ScriptableObject.CreateInstance<Motive>();

    //        if(Enum.TryParse(sprite.name, out Motive.MotiveType type)) 
    //        {
    //            motive.type = type;
    //        }

    //        motive.icon = sprite;
    //        motive.name = sprite.name;

    //        var path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Data/motives/{motive.name}.asset");

    //        //AssetDatabase.AddObjectToAsset(newClue, caseElement);
    //        AssetDatabase.CreateAsset(motive, path);
    //    }


    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //}
}
