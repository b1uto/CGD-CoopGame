using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGD.Case
{
    public abstract class CaseElement : ScriptableObject
    {
        [Header("Element Settings")]

        [ReadOnly]
        public string id;

        [ReadOnly]
        public string caseId;


        [MaxLength(68)] 
        public string shortDescription;
        
        [TextArea(3, 10)]
        public string description;

        /// <summary>
        /// number of clues needed to reveal element
        /// </summary>
        public int cluesToReveal;

        /// <summary>
        /// sprite for display on clue cards and editor
        /// </summary>
        [HideInInspector]
        public Sprite icon;

        /// <summary>
        /// list of all clue ids associated with this element.
        /// </summary>
        [HideInInspector]
        public string[] clues;

        /// <summary>
        /// dictionary for faster lookup at runtime
        /// </summary>
        public Dictionary<string, Clue> clueMap = new Dictionary<string, Clue>();


        //private void OnEnable()
        //{
        //    foreach (var clue in clues) 
        //    {
                
        //    }
        //}

    } 
}
