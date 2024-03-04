using CGD.Case;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Clue))]
public class ClueEditor : Editor
{
    Clue clue;

    private void OnEnable()
    {
        clue = target as Clue;
    }

    public override void OnInspectorGUI()
    {
        if (string.IsNullOrEmpty(clue.id))
        {
            clue.id = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(clue); 
        }

        base.OnInspectorGUI();


        if (clue.icon == null)
            return;


        Texture2D texture = AssetPreview.GetAssetPreview(clue.icon);
        
        GUILayout.Label("", GUILayout.Height(200), GUILayout.Width(200));
        
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}