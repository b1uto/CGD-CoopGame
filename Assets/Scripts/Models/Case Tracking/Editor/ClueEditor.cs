using CGD.Case;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Clue))]
public class ClueEditor : Editor
{
    Clue clue;

    private Sprite[] sprites;
    private int selectedSprite;

    private void OnEnable()
    {
        clue = target as Clue;

        string path = $"Sprites/Clues/";
        sprites = Resources.LoadAll<Sprite>(path);
    }

    public override void OnInspectorGUI()
    {
        if (string.IsNullOrEmpty(clue.id))
        {
            clue.id = System.Guid.NewGuid().ToString();
            EditorUtility.SetDirty(clue); 
        }

        base.OnInspectorGUI();


        if (sprites == null || sprites.Length == 0)
        {
            EditorGUILayout.HelpBox("There are no sprites at the file path", MessageType.Warning);
        }
        else
        {
            selectedSprite = Mathf.Max(0, System.Array.IndexOf(sprites, clue.icon));
            selectedSprite = EditorGUILayout.Popup("Choose Icon", selectedSprite, sprites.Select(x => x.name).ToArray());
            
            if (EditorGUI.EndChangeCheck())
            {
                clue.icon = sprites[selectedSprite];
            }
        }

        if (clue.icon == null)
            return;


        Texture2D texture = AssetPreview.GetAssetPreview(clue.icon);
        
        GUILayout.Label("", GUILayout.Height(200), GUILayout.Width(200));
        
        GUI.DrawTexture(GUILayoutUtility.GetLastRect(), texture);
    }
}