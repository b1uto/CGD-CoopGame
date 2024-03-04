using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using CGD.Case;

[CustomEditor(typeof(CaseElement))]
public class CaseElementEditor : Editor
{
    private string newClueShortDescription = "";
    private Dictionary<string, bool> clueFoldoutStates = new Dictionary<string, bool>();
    private Dictionary<string, Editor> clueEditors = new Dictionary<string, Editor>();

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        CaseElement caseElement = (CaseElement)target;

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
                    // Load the clue using the clue ID and create an editor for it
                    Clue clue = AssetDatabase.LoadAssetAtPath<Clue>(AssetDatabase.GUIDToAssetPath(clueId));
                    clueEditors[clueId] = CreateEditor(clue);
                }

                if (!clueFoldoutStates.ContainsKey(clueId))
                {
                    clueFoldoutStates[clueId] = false; // Default state is closed
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
                        break; // Break to avoid modifying the collection while iterating
                    }
                }
                EditorGUI.indentLevel--;
            }
        }
        GUILayout.Space(20);
        EditorGUILayout.LabelField("-------------------------------------------------", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.LabelField("Create New Clue", EditorStyles.boldLabel);
        newClueShortDescription = EditorGUILayout.TextField("Short Description", newClueShortDescription);

        if (GUILayout.Button("Add Clue"))
        {
            AddClue(caseElement);
        }

        // Add Clue button and other GUI elements remain unchanged

        serializedObject.ApplyModifiedProperties();
    }
    //public override void OnInspectorGUI()
    //{
    //    serializedObject.Update();

    //    CaseElement caseElement = (CaseElement)target;

    //    if (caseElement.clues != null)
    //    {
    //        for (int i = 0; i < caseElement.clues.Length; i++)
    //        {
    //            string clueId = caseElement.clues[i];
    //            Clue clue = AssetDatabase.LoadAssetAtPath<Clue>(AssetDatabase.GUIDToAssetPath(clueId));

    //            if (!clueEditors.ContainsKey(clueId) || clueEditors[clueId] == null)
    //            {
    //                // Create and cache an editor for each clue
    //                Editor editor = CreateEditor(clue);
    //                clueEditors[clueId] = editor;
    //            }

    //            bool foldout = EditorGUILayout.InspectorTitlebar(true, clueEditors[clueId].target);
    //            if (foldout)
    //            {
    //                clueEditors[clueId].OnInspectorGUI();

    //                GUILayout.Space(5);
    //                if (GUILayout.Button("Remove Clue"))
    //                {
    //                    RemoveClue(caseElement, clue, i);
    //                    break; // Break to avoid modifying the list while iterating
    //                }
    //            }
    //        }
    //    }

    //    GUILayout.Space(10);
    //    EditorGUILayout.LabelField("Create New Clue", EditorStyles.boldLabel);
    //    newClueShortDescription = EditorGUILayout.TextField("Short Description", newClueShortDescription);

    //    if (GUILayout.Button("Add Clue"))
    //    {
    //        AddClue(caseElement);
    //    }

    //    serializedObject.ApplyModifiedProperties();
    //}

    private void AddClue(CaseElement caseElement)
    {
        if (string.IsNullOrWhiteSpace(newClueShortDescription))
        {
            Debug.LogWarning("Clue description cannot be empty.");
            return;
        }

        Clue newClue = CreateInstance<Clue>();
        newClue.shortDescription = newClueShortDescription;

        string assetName = caseElement.name + "_" + (caseElement.clues?.Length + 1 ?? 1);
        string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Data/Cases/{assetName}.asset");

        AssetDatabase.CreateAsset(newClue, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        List<string> updatedClues = new List<string>(caseElement.clues) { AssetDatabase.AssetPathToGUID(path) };
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
