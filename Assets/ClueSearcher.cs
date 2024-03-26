using CGD.Case;
using CGD.Gameplay;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class ClueSearcher : MonoBehaviour
{
#if UNITY_EDITOR
    public Clue[] clues;

    public InspectorButton search = new InspectorButton("SearchSceneForClues");
    private List<Clue> clueList;

    private void SearchSceneForClues() 
    {
        clueList = clues.ToList();
        var objs = GameObject.FindObjectsOfType<ClueObject>();

        foreach(var obj in objs) 
        {
            clueList.Remove(obj.clue);
        }

        foreach(var clue in clueList) 
        {
            Debug.Log(clue.name);
        }
    }

#endif
}
