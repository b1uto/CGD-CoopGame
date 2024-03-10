//using UnityEditor;
//using UnityEngine;
//using System.Collections.Generic;
//using System.Linq;
//using CGD.Case;
//using Palmmedia.ReportGenerator.Core.Parser.Analysis;


//[CustomEditor(typeof(CaseFile))]
//public class CaseEditor : Editor
//{
//    private string newClueShortDescription = "";
//    private Dictionary<string, bool> caseElementFoldoutStates = new Dictionary<string, bool>();
//    private Dictionary<string, Editor> caseElementEditors = new Dictionary<string, Editor>();

//    public override void OnInspectorGUI()
//    {
//        serializedObject.Update();

//        CaseFile mainCase = (CaseFile)target;

//        if (string.IsNullOrEmpty(mainCase.id))
//        {
//            mainCase.id = System.Guid.NewGuid().ToString();
//            EditorUtility.SetDirty(mainCase);
//        }


//        EditorGUILayout.LabelField("-------------------- Case File -------------------", EditorStyles.centeredGreyMiniLabel);
//        base.OnInspectorGUI();
//        serializedObject.ApplyModifiedProperties();
//    }
//}
