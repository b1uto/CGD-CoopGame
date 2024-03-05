using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using CGD.Case;


[CustomEditor(typeof(Case))]
public class CaseEditor : Editor
{
    private string newClueShortDescription = "";
    private Dictionary<string, bool> caseElementFoldoutStates = new Dictionary<string, bool>();
    private Dictionary<string, Editor> caseElementEditors = new Dictionary<string, Editor>();

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Case mainCase = (Case)target;

        if (string.IsNullOrEmpty(mainCase.id))
        {
            mainCase.id = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(mainCase);
        }


        EditorGUILayout.LabelField("-------------------- Case File -------------------", EditorStyles.centeredGreyMiniLabel);
        base.OnInspectorGUI();


        EditorGUILayout.Space(20);
        EditorGUILayout.LabelField("--------------------Element Settings-------------------", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.LabelField("Element List", EditorStyles.boldLabel);

        if (mainCase.caseElements != null)
        {
            for (int i = 0; i < mainCase.caseElements.Length; i++)
            {
                string elementId = mainCase.caseElements[i];
                if (!caseElementEditors.ContainsKey(elementId) || caseElementEditors[elementId] == null)
                {
                    CaseElement caseElement = AssetDatabase.LoadAssetAtPath<CaseElement>(AssetDatabase.GUIDToAssetPath(elementId));

                    if (caseElement == null)
                    {
                        RemoveElement(mainCase, elementId);
                        continue;
                    }
                    caseElementEditors[elementId] = CreateEditor(caseElement);
                }

                if (!caseElementFoldoutStates.ContainsKey(elementId))
                {
                    caseElementFoldoutStates[elementId] = false;
                }

                EditorGUI.indentLevel++;
                caseElementFoldoutStates[elementId] = EditorGUILayout.Foldout(caseElementFoldoutStates[elementId], $"{caseElementEditors[elementId].target.name}", true);

                if (caseElementFoldoutStates[elementId])
                {
                    caseElementEditors[elementId].OnInspectorGUI();

                    GUILayout.Space(5);
                    if (GUILayout.Button("Remove Element"))
                    {
                        RemoveElement(mainCase, (CaseElement)caseElementEditors[elementId].target, i);
                        break;
                    }
                }
                EditorGUI.indentLevel--;
            }
        }
        GUILayout.Space(20);
        EditorGUILayout.LabelField("-------------------------------------------------", EditorStyles.centeredGreyMiniLabel);
        EditorGUILayout.LabelField("Create New Element", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Weapon"))
        {
            AddElement<Weapon>(mainCase);
        }
        if (GUILayout.Button("Suspect"))
        {
            AddElement<Suspect>(mainCase);
        }
        if (GUILayout.Button("Motive"))
        {
            AddElement<Motive>(mainCase);
        }

        GUILayout.EndHorizontal();

        // Add Clue button and other GUI elements remain unchanged

        serializedObject.ApplyModifiedProperties();
    }
    private void AddElement<T>(Case mainCase) where T : CaseElement
    {
        CaseElement newElement = CreateInstance<T>();

        newElement.caseId = mainCase.id;

        string assetName = typeof(T).Name + "_" + (mainCase.caseElements?.OfType<T>().Count() + 1 ?? 1);


        System.IO.Directory.CreateDirectory($"Assets/Data/Cases/{mainCase.name}");
        string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Data/Cases/{mainCase.name}/{assetName}.asset");

        AssetDatabase.CreateAsset(newElement, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        List<string> updatedElements = new List<string>(mainCase.caseElements) { AssetDatabase.AssetPathToGUID(path) };
        mainCase.caseElements = updatedElements.ToArray();

        EditorUtility.SetDirty(mainCase);
    }
    private void RemoveElement(Case mainCase, string element)
    {

        List<string> updatedElements = new List<string>(mainCase.caseElements);
        updatedElements.Remove(element);
        mainCase.caseElements = updatedElements.ToArray();
        EditorUtility.SetDirty(mainCase);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    private void RemoveElement(Case mainCase, CaseElement element, int index)
    {


        string path = AssetDatabase.GetAssetPath(element);
        AssetDatabase.DeleteAsset(path);

        List<string> updatedElements = new List<string>(mainCase.caseElements);
        updatedElements.RemoveAt(index);
        mainCase.caseElements = updatedElements.ToArray();
        EditorUtility.SetDirty(mainCase);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
