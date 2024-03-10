using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CGD.Case;
using UnityEditor;
using UnityEngine;

public class DataExtensions : MonoBehaviour
{
    //#region Embed / Unembed Data

    //[MenuItem("Assets/CaseData/Embed Data", true)]
    //private static bool EmbedCaseFileDataValidation()
    //{
    //    // Returns true when the selected object is an AnimatorController (the menu item will be disabled otherwise).
    //    return Selection.activeObject is CaseFile;
    //}

    //[MenuItem("Assets/CaseData/Unembed Data", true)]
    //private static bool UnembedCaseFileDataValidation()
    //{
    //    // Returns true when the selected object is an AnimatorController (the menu item will be disabled otherwise).
    //    return Selection.activeObject is CaseFile;
    //}

    //[MenuItem("Assets/CaseData/Embed Data")]
    //static public void EmbedCaseFileData()
    //{
    //    CaseFile caseFile = null;

    //    List<CaseElement> externalData = new List<CaseElement>();
    //    List<CaseElement> importedData = new List<CaseElement>();//

    //    if (Selection.activeObject.GetType() == typeof(CaseFile))
    //    {
    //        caseFile = (CaseFile)Selection.activeObject;

    //        foreach (var elementId in caseFile.elements)
    //        {
    //            var element = AssetDatabase.LoadAssetAtPath<CaseElement>(AssetDatabase.GUIDToAssetPath(elementId));

    //            if (element != null)
    //            {
    //                //if (importedClips.Where(x => x.name == clip.name).Count() > 0)
    //                //{
    //                //    state.state.motion = importedClips.Find(x => x.name == clip.name);
    //                //    Debug.Log("Using imported clip: <color=orange>" + clip.name.ToString() + "</color> for state: <color=orange>" + state.state.name + "</color>");
    //                //}
    //                //else 
    //                if (AssetDatabase.IsSubAsset(element))
    //                {
    //                    Debug.Log("Clip: <color=orange>" + element.name.ToString() + "</color> is already a sub-asset and will be ignored.");
    //                }
    //                else
    //                {
    //                    var new_element = Object.Instantiate(element);// as AnimationClip;
    //                    new_element.name = element.name;
    //                    AssetDatabase.AddObjectToAsset(new_element, caseFile);
    //                    AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(new_element));
    //                    importedData.Add(new_element);
    //                   // Debug.Log("Added clip: <color=orange>" + new_element.name.ToString() + "</color> to controller: <color=yellow>" + caseFile.name + "</color>");
    //                   //Debug.Log("Using imported clip: <color=orange>" + new_element.name.ToString() + "</color> for state: <color=orange>" + state.state.name + "</color>");
    //                    externalData.Add(element);
    //                }
    //            }
    //            //foreach (var state in layer.stateMachine.states)
    //            //{
                    
    //            //}
    //        }

    //        foreach (var element in externalData)
    //        {
    //            string path = AssetDatabase.GetAssetPath(element);

    //            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(element));
             
    //        }

    //        AssetDatabase.Refresh();
    //    }
    //}

    //[MenuItem("Assets/CaseData/Unembed Data")]
    //static public void UnembedCaseFileData()
    //{
    //    UnityEditor.Animations.AnimatorController animatorController = null;

    //    List<AnimationClip> embeddedClips = new List<AnimationClip>();
    //    List<AnimationClip> exportedClips = new List<AnimationClip>();

    //    if (Selection.activeObject.GetType() == typeof(UnityEditor.Animations.AnimatorController))
    //    {
    //        animatorController = (UnityEditor.Animations.AnimatorController)Selection.activeObject;

    //        string path = AssetDatabase.GetAssetPath(animatorController);
    //        path = path.Substring(0, path.LastIndexOf('/') + 1);

    //        foreach (var layer in animatorController.layers)
    //        {
    //            foreach (var state in layer.stateMachine.states)
    //            {
    //                var clip = state.state.motion as AnimationClip;
    //                if (clip != null)
    //                {
    //                    if (AssetDatabase.IsSubAsset(clip))
    //                    {
    //                        if (exportedClips.Where(x => x.name == clip.name).Count() > 0)
    //                        {
    //                            state.state.motion = exportedClips.Find(x => x.name == clip.name);
    //                            Debug.Log("Using exported clip: <color=orange>" + clip.name.ToString() + "</color> for state: <color=orange>" + state.state.name + "</color>");
    //                        }
    //                        else
    //                        {
    //                            embeddedClips.Add(clip);
    //                            var new_ac = Object.Instantiate(clip) as AnimationClip;
    //                            new_ac.name = clip.name;
    //                            Debug.Log(path);
    //                            var assetPath = path + new_ac.name + ".asset";
    //                            AssetDatabase.CreateAsset(new_ac, assetPath);
    //                            AssetDatabase.ImportAsset(assetPath);
    //                            Debug.Log("Exported clip: <color=orange>" + clip.name.ToString() + "</color> from controller: <color=yellow>" + animatorController.name + "</color> to path:  <color=orange>" + assetPath + "</color>");
    //                            Debug.Log("Using exported clip: <color=orange>" + clip.name.ToString() + "</color> for state: <color=orange>" + state.state.name + "</color>");
    //                            exportedClips.Add(new_ac);
    //                            state.state.motion = new_ac;
    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        foreach (var clip in embeddedClips)
    //        {
    //            AssetDatabase.RemoveObjectFromAsset(clip);
    //            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(animatorController));
    //        }

    //        AssetDatabase.Refresh();
    //    }
    //}
    //#endregion

}