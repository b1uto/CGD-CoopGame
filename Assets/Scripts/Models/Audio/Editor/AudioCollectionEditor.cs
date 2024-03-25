using CGD.Audio;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioCollection), true)] 
public class AudioCollectionEditor : Editor
{
    private AudioCollection audioCollection;

    PropertyInfo namesField;
    //FieldInfo clipsField;

    private string[] names;
    private List<AudioClip> clips = new List<AudioClip>();

    private void OnEnable()
    {
        audioCollection = target as AudioCollection; 
       
        //clipsField = typeof(AudioCollection).GetField("clips", BindingFlags.NonPublic | BindingFlags.Instance);
        namesField = typeof(AudioCollection).GetProperty("Names", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        names = (string[])namesField.GetValue(audioCollection);
        clips = new List<AudioClip>(names.Length);

        if (audioCollection.clips != null)
            clips = audioCollection.clips.ToList();

        //var value = (AudioClip[])clipsField.GetValue(audioCollection);

        //if(value != null ) 
        //{
        //    clips = value.ToList();
        //}
    }

    public override void OnInspectorGUI() 
    {
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Sound List");
        EditorGUI.indentLevel++;
        if (names != null)
        {
            for (int i = 0; i < names.Length; ++i)
            {
                if(i >= clips.Count) clips.Add(null);

                clips[i] = (AudioClip)EditorGUILayout.ObjectField(names[i], clips[i], typeof(AudioClip), true);   
            }
        }

        EditorGUI.indentLevel--;

        if (EditorGUI.EndChangeCheck()) 
        {
            audioCollection.clips = clips.ToArray();
           // clipsField.SetValue(audioCollection, clips.ToArray());


            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }



        serializedObject.ApplyModifiedProperties();
        EditorUtility.SetDirty(target);
    }
}


/*  for(int i = 0; i < names.Length; ++i) 
            {
                if (i >= clips.Count) clips.Add(null);

                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], names[i]);

                if (foldouts[i])
                {
                    if (clips[i] != null)
                    {
                        for (int j = 0; j < clips[i].Count; j++)
                        {
                            clips[i][j] = (AudioClip)EditorGUILayout.ObjectField($"Clip {j}", clips[i][j], typeof(AudioClip), false);
                        }
                    }

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("+")) 
                    {
                        clips[i].Add(null);
                    }
                    if (GUILayout.Button("-"))
                    {
                        clips[i].RemoveAt(clips[i].Count-1);
                    }
                    EditorGUILayout.EndHorizontal();

                }
            }*/



/*BACKUPPP
 
 using CGD.Audio;
using PlayFab.ClientModels;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioCollection), false)] 
public class AudioCollectionEditor : Editor
{
    private AudioCollection audioCollection;

    PropertyInfo namesField;
    FieldInfo clipsField;

    private string[] names;
    private List<List<AudioClip>> clips = new List<List<AudioClip>>();

    private bool[] foldouts;

    private void OnEnable()
    {
        audioCollection = target as AudioCollection; 
       
        clipsField = typeof(AudioCollection).GetField("clips", BindingFlags.NonPublic | BindingFlags.Instance);
        namesField = typeof(AudioCollection).GetProperty("Names", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy);

        //type = (string)typeField.GetValue(audioCollection);
        clips = new List<List<AudioClip>>();
        names = (string[])namesField.GetValue(audioCollection);

        foldouts = new bool[names.Length];

        var value = (AudioClip[][])clipsField.GetValue(audioCollection);

        //
        if(value != null ) 
        {
            foreach(var collection in value) 
            {
                clips.Add(collection.ToList());
            }
        }
        else 
        {
            for(int i = 0; i < names.Length; i++) 
            {
                clips.Add(new List<AudioClip>() {null });
            }
        }
    }

    public override void OnInspectorGUI() 
    {
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.LabelField("Sound List");
        EditorGUI.indentLevel++;
        if (names != null)
        {
            for (int i = 0; i < names.Length; ++i)
            {
                if (i >= clips.Count) clips.Add(null);

                EditorGUILayout.LabelField(names[i]);

                EditorGUI.indentLevel++;

                if (clips[i] != null)
                {
                    for (int j = 0; j < clips[i].Count; j++)
                    {
                        clips[i][j] = (AudioClip)EditorGUILayout.ObjectField($"Clip {j}", clips[i][j], typeof(AudioClip), false);
                    }
                }

                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("+"))
                {
                    clips[i].Add(null);
                }
                if (GUILayout.Button("-"))
                {
                    clips[i].RemoveAt(clips[i].Count - 1);
                }
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;

            }

        }

        EditorGUI.indentLevel--;

        if (EditorGUI.EndChangeCheck()) 
        {
            for(int i = clips.Count - 1; i >= 0; i--) 
            {
                if (clips[i] == null)
                    clips.Remove(clips[i]);
            }

            var val = clips.Select(subList => subList.ToArray()).ToArray();

            clipsField.SetValue(audioCollection, val);
        
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(target);
        }
    }
}


/*  for(int i = 0; i < names.Length; ++i) 
            {
                if (i >= clips.Count) clips.Add(null);

                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], names[i]);

                if (foldouts[i])
                {
                    if (clips[i] != null)
                    {
                        for (int j = 0; j < clips[i].Count; j++)
                        {
                            clips[i][j] = (AudioClip)EditorGUILayout.ObjectField($"Clip {j}", clips[i][j], typeof(AudioClip), false);
                        }
                    }

                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("+")) 
                    {
                        clips[i].Add(null);
                    }
                    if (GUILayout.Button("-"))
                    {
                        clips[i].RemoveAt(clips[i].Count-1);
                    }
                    EditorGUILayout.EndHorizontal();

                }
            }*/
