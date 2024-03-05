using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using CGD.Case;
using System.Linq;

[CustomEditor(typeof(CaseElement))]
public class CaseElementEditor : Editor
{
    private string newClueShortDescription = "";
    private Dictionary<string, bool> clueFoldoutStates = new Dictionary<string, bool>();
    private Dictionary<string, Editor> clueEditors = new Dictionary<string, Editor>();

    private Case caseFile;

    private void FindCaseFile(CaseElement element)
    {
        List<Case> cases = new List<Case>();

        // FindAssets uses filters, "t:Case" finds all Case assets
        string[] guids = AssetDatabase.FindAssets("t:Case");

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Case foundCase = AssetDatabase.LoadAssetAtPath<Case>(assetPath);

            if (foundCase != null)
            {
                cases.Add(foundCase);
            }
        }

        caseFile = cases.FirstOrDefault(x => x.id == element.caseId);
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CaseElement caseElement = (CaseElement)target;

        FindCaseFile(caseElement);

        if (string.IsNullOrEmpty(caseElement.id))
        {
            caseElement.id = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(caseElement);
        }


        EditorGUILayout.LabelField("--------------------Case Element-------------------", EditorStyles.centeredGreyMiniLabel);
        base.OnInspectorGUI();

        
        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("--------------------Clue Settings-------------------", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.LabelField("Clue List", EditorStyles.boldLabel);

        if (caseElement.clues != null)
        {
            for (int i = 0; i < caseElement.clues.Length; i++)
            {
                string clueId = caseElement.clues[i];
                if (!clueEditors.ContainsKey(clueId) || clueEditors[clueId] == null)
                {
                    Clue clue = AssetDatabase.LoadAssetAtPath<Clue>(AssetDatabase.GUIDToAssetPath(clueId));
                    clueEditors[clueId] = CreateEditor(clue);
                }

                if (!clueFoldoutStates.ContainsKey(clueId))
                {
                    clueFoldoutStates[clueId] = false; 
                }

                EditorGUI.indentLevel++;
                clueFoldoutStates[clueId] = EditorGUILayout.Foldout(clueFoldoutStates[clueId], $"{clueEditors[clueId].target.name}", true);

                if (clueFoldoutStates[clueId])
                {
                    clueEditors[clueId].OnInspectorGUI();

                    GUILayout.Space(5);
                    if (GUILayout.Button("Remove Clue"))
                    {
                        RemoveClue(caseElement, (Clue)clueEditors[clueId].target, i);
                        break; 
                    }
                }
                EditorGUI.indentLevel--;
            }
        }

        if (GUILayout.Button("Add Clue"))
        {
            AddClue(caseElement);
        }

        // Add Clue button and other GUI elements remain unchanged

        serializedObject.ApplyModifiedProperties();
    }
    private void AddClue(CaseElement caseElement)
    {
        if(caseFile == null) 
        {
            Debug.LogWarning("Case File Not Found");
            return;
        }

        Clue newClue = CreateInstance<Clue>();
        newClue.elementId = caseElement.id;

        string assetName = "Clue_" + (caseElement.clues?.Length + 1);

        System.IO.Directory.CreateDirectory($"Assets/Data/Cases/{caseFile.name}/{caseElement.name}");
        string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Data/Cases/{caseFile.name}/{caseElement.name}/{assetName}.asset");

        AssetDatabase.CreateAsset(newClue, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();


        List<string> updatedClues = caseElement.clues == null ? new List<string>() : new List<string>(caseElement.clues);
        updatedClues.Add(AssetDatabase.AssetPathToGUID(path));

        caseElement.clues = updatedClues.ToArray();
        newClueShortDescription = "";
        EditorUtility.SetDirty(caseElement);
    }

    private void RemoveClue(CaseElement caseElement, Clue clue, int index)
    {
        string path = AssetDatabase.GetAssetPath(clue);
        AssetDatabase.DeleteAsset(path);

        List<string> updatedClues = new List<string>(caseElement.clues);
        updatedClues.RemoveAt(index);
        caseElement.clues = updatedClues.ToArray();
        EditorUtility.SetDirty(caseElement);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
