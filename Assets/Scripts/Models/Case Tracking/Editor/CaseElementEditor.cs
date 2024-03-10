using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using CGD.Case;
using System.Linq;
using ExitGames.Client.Photon;
using static UnityEditor.Rendering.FilterWindow;

[CustomEditor(typeof(CaseElement))]
public class CaseElementEditor : Editor
{
    private Dictionary<string, bool> clueFoldoutStates = new Dictionary<string, bool>();
    private Dictionary<string, Editor> clueEditors = new Dictionary<string, Editor>();

    private CaseElement targetElement;
    private CaseFile caseFile;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        targetElement = (CaseElement)target;

        CaseData.GenerateUniqueID(targetElement);

        if (caseFile == null)
            GetCaseFile();


        EditorGUILayout.Space(20);

        EditorGUILayout.LabelField("--------------------Case Element-------------------", EditorStyles.centeredGreyMiniLabel);
        base.OnInspectorGUI();

        EditorGUILayout.Space(20);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space(50);
        EditorGUILayout.BeginVertical();
        EditorGUILayout.LabelField("--------------------Clue Settings-------------------", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.LabelField("Clue List", EditorStyles.boldLabel);

        if (targetElement.clues != null && targetElement.clues.Length > 0)
        {
            var clueAssetPaths = System.IO.Directory.GetFiles($"Assets/Data/Cases/{caseFile.name}/{targetElement.name}/", "*.asset");//.OfType<Clue>().ToArray();

            var clueAssets = clueAssetPaths.Select(x => AssetDatabase.LoadAssetAtPath<Clue>(x)).ToArray();

            if (clueAssets != null && clueAssets.Length > 0)
            {
                foreach (var clue in targetElement.clues)
                {
                    var obj = clueAssets.FirstOrDefault(x => x.id == clue);
                    if (obj == null) continue;

                    if (!clueEditors.ContainsKey(clue) || clueEditors[clue] == null)
                    {
                        clueEditors[clue] = CreateEditor(obj);
                    }

                    if (!clueFoldoutStates.ContainsKey(clue))
                    {
                        clueFoldoutStates[clue] = false;
                    }

                    EditorGUI.indentLevel++;
                    clueFoldoutStates[clue] = EditorGUILayout.Foldout(clueFoldoutStates[clue], $"{obj.name}", true);

                    if (clueFoldoutStates[clue])
                    {
                        clueEditors[clue].OnInspectorGUI();

                        GUILayout.Space(10);
                        if (GUILayout.Button("Remove Clue"))
                        {
                            RemoveClue((Clue)clueEditors[clue].target);
                            break;
                        }
                    }
                    EditorGUI.indentLevel--;
                }
            }
        }

        if (GUILayout.Button("Add Clue"))
        {
            AddClue();
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space(50);
        EditorGUILayout.EndHorizontal();



        serializedObject.ApplyModifiedProperties();
    }

    private void GetCaseFile()
    {
        List<CaseFile> cases = new List<CaseFile>();

        foreach (string guid in AssetDatabase.FindAssets("t:CaseFile"))
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            CaseFile foundCase = AssetDatabase.LoadAssetAtPath<CaseFile>(assetPath);

            if (foundCase != null)
                cases.Add(foundCase);
        }

        caseFile = cases.FirstOrDefault(x => x.id == ((CaseElement)target).caseId);
    }

    private void AddClue()
    {
       if(caseFile == null)
            GetCaseFile();

        Clue newClue = CreateInstance<Clue>();
        newClue.elementId = targetElement.id;
        newClue.name = targetElement.name+"_Clue_"+targetElement.clues.Length;

        System.IO.Directory.CreateDirectory($"Assets/Data/Cases/{caseFile.name}/{targetElement.name}");
        string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Data/Cases/{caseFile.name}/{targetElement.name}/{newClue.name}.asset");

        //AssetDatabase.AddObjectToAsset(newClue, caseElement);
        AssetDatabase.CreateAsset(newClue, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        List<string> updatedClues =  new List<string>(targetElement.clues);
        updatedClues.Add(newClue.ID);

        targetElement.clues = updatedClues.ToArray();
        EditorUtility.SetDirty(targetElement);
    }

    private void RemoveClue(Clue clue)
    {
        List<string> updatedClues = new List<string>(targetElement.clues);
        updatedClues.Remove(clue.id);
        targetElement.clues = updatedClues.ToArray();

        string path = AssetDatabase.GetAssetPath(clue);
        AssetDatabase.DeleteAsset(path);

        EditorUtility.SetDirty(targetElement);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void DeleteElement() 
    {
        AssetDatabase.DeleteAsset($"Assets/Data/Cases/{caseFile.name}/{targetElement.name}");
        
        string element = AssetDatabase.GetAssetPath(target);
        AssetDatabase.DeleteAsset(element);
    }
}
