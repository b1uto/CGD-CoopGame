using CGD.Case;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public class CaseWindowEditor : EditorWindow
{
    private string newClueShortDescription = "";
    private Dictionary<string, bool> caseElementFoldoutStates = new Dictionary<string, bool>();
    private Dictionary<string, Editor> caseElementEditors = new Dictionary<string, Editor>();
    private Case mainCase;

    private int selectedIndex;

    [MenuItem("Window/CGD/Case File Editor")]
    public static void ShowWindow()
    {
        GetWindow<CaseWindowEditor>("Case File Editor");
    }

    private void OnGUI()
    {

        EditorGUILayout.BeginHorizontal(); // Main content area split into left and right

        // Left panel
        EditorGUILayout.BeginVertical(GUILayout.Width(400)); // Fixed width for left panel
        GUILayout.Label("--- Case Settings ---", EditorStyles.boldLabel);
        List<Case> cases = new List<Case>();

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

        var caseNames = cases.Select(x => x.name).ToArray();

        int newCaseIndex = EditorGUILayout.Popup("Select Case:", selectedIndex, caseNames);
        if (newCaseIndex != selectedIndex)
        {
            selectedIndex = newCaseIndex;
            mainCase = cases[selectedIndex];
        }
        EditorGUILayout.EndVertical(); // End of Left Panel

        // Right panel
        EditorGUILayout.BeginVertical(GUILayout.Width(800)); // Takes the remaining space
        GUILayout.Label("Right Panel", EditorStyles.boldLabel);
        if (GUILayout.Button("Button 2"))
        {
            Debug.Log("Right panel Button 2 clicked");
        }
        EditorGUILayout.EndVertical(); // End of Right Panel

        EditorGUILayout.EndHorizontal(); // End of Main content area

        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.LabelField("Case File");
        //mainCase = (Case)EditorGUILayout.ObjectField(mainCase, typeof(Case), false);
        //EditorGUILayout.EndHorizontal();

        if (mainCase == null)
        {
            EditorGUILayout.HelpBox("Select a Case object to edit.", MessageType.Info);
            return;
        }

        if (string.IsNullOrEmpty(mainCase.id))
        {
            mainCase.id = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(mainCase);
        }


        EditorGUILayout.LabelField("-------------------- Case File -------------------", EditorStyles.centeredGreyMiniLabel);
        //base.OnInspectorGUI();


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
                    caseElementEditors[elementId] = Editor.CreateEditor(caseElement);
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
