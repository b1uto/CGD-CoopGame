using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

[CustomEditor(typeof(BuildSettings))]
[InitializeOnLoad]
public class BuildSettingsEditor : Editor
{
    static bool changeRequested;
    static bool changeConfirmed;
    static bool recompileFinished;

    static BuildSettings.BuildTarget confirmationLabelPreviousTarget;
    static BuildSettings.BuildTarget confirmationLabelNewTarget;

    bool showScriptingDefineSymbols = false;

    /// <summary>
    /// Add define symbols
    /// </summary>
    static void AddDefineSymbols(string[] symbolsToAdd)
    {
        //string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';'));//definesString.Split(';').ToList();

        foreach (string symbol in symbolsToAdd)
        {
            if (allDefines.Contains(symbol))
            {
                // already defined, skip...
                continue;
            }
            else
            {
                allDefines.Add(symbol);
            }
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }

    /// <summary>
    /// Remove define symbols
    /// </summary>
    static void RemoveDefineSymbols(string[] symbolsToRemove)
    {
        //string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';'));//definesString.Split(';').ToList();

        foreach (string symbol in symbolsToRemove)
        {
            if (!allDefines.Contains(symbol))
            {
                // not defined, skip...
                continue;
            }
            else
            {
                allDefines.Remove(symbol);
            }
        }

        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }

    /// <summary>
    /// Remove then add define symbols
    /// </summary>
    static void SetDefineSymbols(string[] symbolsToRemove, string[] symbolsToAdd)
    {
        //string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        List<string> allDefines = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';'));//definesString.Split(';').ToList();

        foreach (string symbol in symbolsToRemove)
        {
            if (!allDefines.Contains(symbol))
            {
                // not defined, skip...
                continue;
            }
            else
            {
                allDefines.Remove(symbol);
            }
        }

        foreach (string symbol in symbolsToAdd)
        {
            if (allDefines.Contains(symbol))
            {
                // already defined, skip...
                continue;
            }
            else
            {
                allDefines.Add(symbol);
            }
        }
        PlayerSettings.SetScriptingDefineSymbolsForGroup(
            EditorUserBuildSettings.selectedBuildTargetGroup,
            string.Join(";", allDefines.ToArray()));
    }

    /// <summary>
    /// Get the current server target from the scripting define symbols
    /// </summary>
    /// <returns>Server Target value</returns>
    static BuildSettings.BuildTarget GetCurrentTarget()
    {
        List<string> allDefines = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';'));
        BuildSettings.BuildTarget target = BuildSettings.BuildTarget.Undefined;
        bool containsDevelopmentDefinition = allDefines.Contains(BuildSettings.SDS_Development);
        bool containsProductionDefinition = allDefines.Contains(BuildSettings.SDS_Production);

        if (containsDevelopmentDefinition && !containsProductionDefinition)
        {
            target = BuildSettings.BuildTarget.Development;
            return target;
        }
        else if (!containsDevelopmentDefinition && containsProductionDefinition)
        {
            target = BuildSettings.BuildTarget.Production;
            return target;
        }

        return target;
    }

    static BuildSettingsEditor()
    {
        if (changeConfirmed)
        {
            recompileFinished = true;
        }
    }

    private void OnEnable()
    {
        ((BuildSettings)target).buildTarget = GetCurrentTarget();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var currentTarget = ((BuildSettings)target).buildTarget;

        if (changeRequested && !changeConfirmed)
        {
            EditorGUILayout.HelpBox("Change Server Target from " +
                confirmationLabelPreviousTarget.ToString() +
                " to " +
                confirmationLabelNewTarget.ToString() +
                "?", MessageType.Warning);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("OK", EditorStyles.toolbarButton))
            {
                UpdateBuildTarget(confirmationLabelNewTarget);
                changeConfirmed = true;
            }
            if (GUILayout.Button("Cancel", EditorStyles.toolbarButton))
            {
                changeRequested = false;
            }
            EditorGUILayout.EndHorizontal();
        }
        else if (changeConfirmed && !recompileFinished)
        {
            // recompile in progress
            EditorGUILayout.LabelField("Recompiling...");
        }
        else if (changeConfirmed && recompileFinished)
        {
            // update has completed, safe to show dropdown again
            changeConfirmed = false;
            changeRequested = false;
            Repaint();
        }
        else
        {

            // server target setting
            Array targets = Enum.GetValues(typeof(BuildSettings.BuildTarget));
            List<int> targetIndices = new List<int>();
            List<GUIContent> targetNames = new List<GUIContent>();
            for (int x = 0; x < targets.Length; x++)
            {
                targetNames.Add(new GUIContent(targets.GetValue(x).ToString()));
                targetIndices.Add(x);
            }

            var previousTargetIndex = (int)currentTarget;

            var selectedTargetIndex = EditorGUILayout.IntPopup(new GUIContent("Server Target*", "Changing the Server Target updates the scripting define symbols as specified in the script."), previousTargetIndex, targetNames.ToArray(), targetIndices.ToArray());

            if (selectedTargetIndex != previousTargetIndex)
            {
                confirmationLabelPreviousTarget = currentTarget;
                confirmationLabelNewTarget = (BuildSettings.BuildTarget)selectedTargetIndex;
                changeRequested = true;
            }

            // show scripting define symbols
            showScriptingDefineSymbols = EditorGUILayout.Foldout(showScriptingDefineSymbols, new GUIContent("Scripting Define Symbols*", "Scripting define symbols are preprocessor directives defined in Player Settings."));
            if (showScriptingDefineSymbols)
            {
                foreach (string symbol in PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup).Split(';'))
                {
                    EditorGUILayout.HelpBox(symbol, MessageType.None);
                }
            }
        }
    }

    /// <summary>
    /// Updates the scripting define symbols as defined in the constants
    /// </summary>
    /// <param name="newTarget">The new Server Target</param>
    private void UpdateBuildTarget(BuildSettings.BuildTarget newTarget)
    {
        var allDefines = new string[] { BuildSettings.SDS_Development, BuildSettings.SDS_Production };

        switch (newTarget)
        {
            case BuildSettings.BuildTarget.Development:
                SetDefineSymbols(allDefines, new string[] { BuildSettings.SDS_Development });
                break;
            case BuildSettings.BuildTarget.Production:
                SetDefineSymbols(allDefines, new string[] { BuildSettings.SDS_Production });
                break;
        }
    }
}
