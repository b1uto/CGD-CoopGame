using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGD.Case
{
    public abstract class CaseElement : CaseData
    {
        [Header("Element Settings")]

        [ReadOnly]
        public string caseId;

        /// <summary>
        /// number of clues needed to reveal element
        /// </summary>
        public int cluesToReveal;

        /// <summary>
        /// list of all clue ids associated with this element.
        /// </summary>
        [HideInInspector]
        public string[] clues;


        public abstract CaseItem GetItem();
    } 
}
