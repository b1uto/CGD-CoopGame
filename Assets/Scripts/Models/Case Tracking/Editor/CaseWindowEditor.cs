using CGD.Case;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CaseWindowEditor : EditorWindow
{
    //private string newClueShortDescription = "";
    private Dictionary<string, bool> elementFoldoutStates = new Dictionary<string, bool>();
    private Dictionary<string, Editor> elementEditors = new Dictionary<string, Editor>();
    private CaseFile mainCase;

    private float splitPosition = 200;
    private bool isResizing = false;
    private Rect splitterRect;
    private Vector2 scrollPositionL;
    private Vector2 scrollPositionR;


    private string newCaseFileName;

    private Editor caseEditor;

    private Color defaultColor;


    [MenuItem("CGD Tools/Case File Editor")]
    public static void ShowWindow()
    {
        var window =    GetWindow<CaseWindowEditor>("Case File Editor");
        window.minSize = new Vector2(820, 600);
        window.maxSize = new Vector2(820, 600);
    }

    public void OnEnable()
    {
        defaultColor = GUI.backgroundColor;
    }


    private string[] LoadCaseFiles(ref List<CaseFile> cases) 
    {
        string[] guids = AssetDatabase.FindAssets("t:Case");

        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            CaseFile foundCase = AssetDatabase.LoadAssetAtPath<CaseFile>(assetPath);

            if (foundCase != null)
            {
                cases.Add(foundCase);
            }
        }

      return cases.Select(x => x.name).ToArray();

    }


    private void DrawCaseFileSelection()
    {
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;

        GUILayout.Label("Case File Select", style);
        GUILayout.Space(10);


        if (AssetDatabase.FindAssets("t:CaseFile").Length > 0)
        {
            mainCase = (CaseFile)EditorGUILayout.ObjectField("Select Case", mainCase, typeof(CaseFile), false);

            if (mainCase != null)
            {
                GUILayout.Space(5);
                if (GUILayout.Button("Delete", GUILayout.MinWidth(80)))
                {
                    DeleteCaseFile(mainCase);
                }
                // EditorGUILayout.EndHorizontal();
            }
            GUILayout.Space(15);
        }

        GUILayout.Label("Create New Case");
        EditorGUILayout.BeginHorizontal();
        newCaseFileName = EditorGUILayout.TextField(newCaseFileName);
        if (GUILayout.Button("Create", GUILayout.MinWidth(80))) // string.IsNullOrEmpty(newCaseFileName) ? disabledButtonStyle : normalButtonStyle))
        {
            CreateCaseFile();
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(5);


    }

    private void DrawCaseFile() 
    {
        if (mainCase != null)
        {
            if (caseEditor == null || caseEditor.target != mainCase)
            {
                if (caseEditor != null) DestroyImmediate(caseEditor);

                caseEditor = Editor.CreateEditor(mainCase);
            }

            caseEditor.OnInspectorGUI();

            EditorGUILayout.LabelField("-------------------------------------------------", EditorStyles.centeredGreyMiniLabel);
            EditorGUILayout.LabelField("Create New Element", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Weapon"))
            {
                AddElement<Weapon>();
            }
            if (GUILayout.Button("Suspect"))
            {
                AddElement<Suspect>();
            }
            if (GUILayout.Button("Motive"))
            {
                AddElement<Motive>();
            }

            GUILayout.EndHorizontal();
        }
    }

    private void DrawCaseElements() 
    {
        if (mainCase.elements != null)
        {
            var foldoutStyle = new GUIStyle(EditorStyles.foldout)
            {
                fontStyle = FontStyle.Bold,
                fontSize = 12
            };

            string assetPath = AssetDatabase.GetAssetPath(mainCase);
            var allElements = AssetDatabase.LoadAllAssetsAtPath(assetPath).OfType<CaseElement>().ToArray();


            foreach (var element in mainCase.elements)
            {
                var obj = allElements.FirstOrDefault(x => x.id == element);

                if (obj == null || string.IsNullOrEmpty(element)) continue;


                if (!elementEditors.ContainsKey(element) || elementEditors[element] == null)
                {
                    elementEditors[element] = Editor.CreateEditor(obj);
                }

                if (!elementFoldoutStates.ContainsKey(element))
                {
                    elementFoldoutStates[element] = false;
                }

                EditorGUI.indentLevel++;


                elementFoldoutStates[element] = EditorGUILayout.Foldout(elementFoldoutStates[element], $"{obj.name}", true, foldoutStyle);
            
                if (elementFoldoutStates[element])
                {
                    elementEditors[element].OnInspectorGUI();
                    GUILayout.Space(5);
                    

                    if (GUILayout.Button("Remove Element"))
                    {
                        RemoveElement(obj);
                        break;
                    }
                }
                EditorGUI.indentLevel--;
            }
        }
    }

    private void DrawLeftPanel() 
    {
        EditorGUILayout.BeginVertical(GUILayout.Width(splitPosition - 10));
        scrollPositionL = GUILayout.BeginScrollView(scrollPositionL);
        DrawCaseFileSelection();
        DrawCaseFile();
        GUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }

    private void DrawRightPanel()
    {
        if (mainCase == null) return;

        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(10);
        scrollPositionR = GUILayout.BeginScrollView(scrollPositionR);

        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleCenter;

        GUILayout.Label("Case Elements", style);
        GUILayout.Space(10);

        DrawCaseElements();

        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
    }




    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal(GUILayout.ExpandHeight(true));

        DrawLeftPanel();

        HandleSplitter();
        GUILayout.Space(5);

        DrawRightPanel();
        //GUILayout.Box("Right Panel", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        
        EditorGUILayout.EndHorizontal();
    }

    private void HandleSplitter()
    {
       // GUILayout.Space(2);
        splitterRect = new Rect(splitPosition, 0, 2, position.height);

        // Draw the splitter as a draggable area.
        EditorGUIUtility.AddCursorRect(splitterRect, MouseCursor.ResizeHorizontal);
        if (Event.current.type == EventType.MouseDown && splitterRect.Contains(Event.current.mousePosition))
        {
            isResizing = true;
        }

        if (isResizing)
        {
            splitPosition = Event.current.mousePosition.x;
            splitPosition = Mathf.Clamp(splitPosition, 320, position.width - 300);
            Repaint();
        }

        if (Event.current.type == EventType.MouseUp)
        {
            isResizing = false;
        }

        EditorGUI.DrawRect(splitterRect, new Color(0.5f, 0.5f, 0.5f, 1.0f));
        //GUILayout.Space(2);

    }


    private void CreateCaseFile()
    {
        if(string.IsNullOrEmpty(newCaseFileName))
        {
            Debug.LogWarning("Case File Now must not be null");
        }

        if (AssetDatabase.LoadAssetAtPath<CaseFile>($"Assets/Data/Cases/{newCaseFileName}") != null)
        {
            Debug.LogWarning("CaseFile with same name already exists");
        }

        string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Data/Cases/{newCaseFileName}.asset");

        CaseFile newCase = CreateInstance<CaseFile>();


        CaseData.GenerateUniqueID(newCase);
 
        AssetDatabase.CreateAsset(newCase, path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        mainCase = newCase;
        EditorUtility.SetDirty(mainCase);
    }
    private void DeleteCaseFile(CaseFile caseToDelete)
    {
        if (EditorUtility.DisplayDialog("Delete Case File",
            $"Are you sure you want to delete {caseToDelete.name}?", "Delete", "Cancel"))
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(caseToDelete));
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            mainCase = null;
        }
    }


    private void AddElement<T>() where T : CaseElement
    {
        CaseElement newElement = CreateInstance<T>();
        newElement.name = typeof(T).Name;
        newElement.caseId = mainCase.ID;

        // string path = AssetDatabase.GenerateUniqueAssetPath($"Assets/Data/Cases/{assetName}.asset");

        AssetDatabase.AddObjectToAsset(newElement, mainCase); 
        
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(newElement));


        List<string> updatedElements = mainCase.elements == null ? new List<string>() : new List<string>(mainCase.elements);
        updatedElements.Add(newElement.ID);

        mainCase.elements = updatedElements.ToArray();

        EditorUtility.SetDirty(mainCase);
        EditorUtility.SetDirty(newElement);

    }
    private void RemoveElement(CaseElement element)
    {
        List<string> updatedElements = new List<string>(mainCase.elements);
        updatedElements.Remove(element.id);
        mainCase.elements = updatedElements.ToArray();

        if (AssetDatabase.IsSubAsset(element))
            AssetDatabase.RemoveObjectFromAsset(element);

        if (elementEditors.ContainsKey(element.id))
            elementEditors.Remove(element.id);

        if (elementFoldoutStates.ContainsKey(element.id))
            elementFoldoutStates.Remove(element.id);


        EditorUtility.SetDirty(mainCase);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
