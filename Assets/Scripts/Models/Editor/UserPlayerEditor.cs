//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;
//using UnityEngine.UIElements;

//[CustomEditor(typeof(UserPlayer))]
//public class UserPlayerEditor : EditorWindow
//{
//    UserPlayer userPlayer;
//    Color color; //TODO skin color?
//    GameObject gameObject;
//    Editor gameObjectEditor;

//    private VisualElement m_RightPane;
//    const string baseAssetPath = "Assets/Data/UserPlayers/";

//    [MenuItem("Tools/User Player Editor")]
//    public static void ShowMyEditor()
//    {
//        EditorWindow wnd = GetWindow<UserPlayerEditor>();
//        wnd.titleContent = new GUIContent("Character Editor");


//        wnd.minSize = new Vector2(450, 200);
//        wnd.maxSize = new Vector2(1920, 720);
//    }

//    public void CreateGUI()
//    {
//        string[] guids = AssetDatabase.FindAssets("t:gameobject", new[] { "Assets/Resources/Models" });

//        var objects = new List<GameObject>();
//        foreach (string obj in guids)
//        {
//            objects.Add(AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(obj)));
//        }
//        //var objectList = new List<GameObject>();
//        //foreach (var guid in objects)
//        //{
//        //    objectList.Add(AssetDatabase.LoadAssetAtPath<Sprite>(AssetDatabase.GUIDToAssetPath(guid)));
//        //}

//        // Create a two-pane view with the left pane being fixed with
//        var splitView = new TwoPaneSplitView(0, 250, TwoPaneSplitViewOrientation.Horizontal);

//        // Add the panel to the visual tree by adding it as a child to the root element
//        rootVisualElement.Add(splitView);

//        // A TwoPaneSplitView always needs exactly two child elements
//        var leftPane = new ListView();
//        splitView.Add(leftPane);
//        m_RightPane = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
//        splitView.Add(m_RightPane);

//        // Initialize the list view with all sprites' names
//        leftPane.makeItem = () => new Label();
//        leftPane.bindItem = (item, index) => { (item as Label).text = objects[index].name; };
//        leftPane.itemsSource = objects;

//        // React to the user's selection
//        leftPane.selectionChanged += OnModelSelectionChange;

//        // Restore the selection index from before the hot reload
//        //leftPane.selectedIndex = m_SelectedIndex;

//        // Store the selection index when the selection changes
//       // leftPane.selectionChanged += (items) => { m_SelectedIndex = leftPane.selectedIndex; };
//    }


//    void OnModelSelectionChange(object obj)
//    {
//        var gameObject = obj as GameObject;

//        GUIStyle bgColor = new GUIStyle();
//        bgColor.normal.background = EditorGUIUtility.whiteTexture;

//        if (gameObject != null)
//        {
//            if (gameObjectEditor == null)
//                gameObjectEditor = Editor.CreateEditor(gameObject);

//            gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
//            VisualElement visualElement = GUILayoutUtility.GetRect(256, 256);


//            m_RightPane.Add(gameObjectEditor);
//        }
//    }

//    //void OnGUI()
//    //{
//    //    gameObject = (GameObject)EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);

//    //    GUIStyle bgColor = new GUIStyle();
//    //    bgColor.normal.background = EditorGUIUtility.whiteTexture;

//    //    if (gameObject != null)
//    //    {
//    //        if (gameObjectEditor == null)
//    //            gameObjectEditor = Editor.CreateEditor(gameObject);

//    //        gameObjectEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(256, 256), bgColor);
//    //    }
//    //}

//    //public override bool HasPreviewGUI()
//    //{
//    //    return true;
//    //}
//}





////private void DevelopmentServerManager_OnItemInserted(Type itemType, string shortCodeIndex)
////{
////    if (itemType == typeof(Boots))
////    {
////        //Extract the ShortCode and Index
////        string[] keyValue = shortCodeIndex.Split('=');
////        string shortCode = keyValue[0];
////        int serverIndex = Convert.ToInt32(keyValue[1]);

////        //Load the Asset from the Database
////        Boots targetItem = AssetDatabase.LoadAssetAtPath<Boots>(baseAssetPath + shortCode + ".asset");
////        if (targetItem != null)
////        {
////            //Update the Id
////            targetItem.Id = serverIndex;
////        }
////    }
////}