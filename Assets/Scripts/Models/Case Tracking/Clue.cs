using UnityEngine;

namespace CGD.Case
{
    #region Runtime

    public struct ClueInfo 
    {
        public bool communityClue;
        public ClueStatus status;
        
        public ClueInfo(bool  communityClue, ClueStatus status)
        {
            this.communityClue = communityClue;
            this.status = status;
        }
    }

    public enum ClueStatus
    {
        Discovered,
        Collected, 
        Analysed
    }
    #endregion

    public enum IdentifyTool
    {
        None,
        UVLight,
        Luminol,
        MagnifyingGlass,
        MetalDetector,
        FlashLight,
    }

    public enum AnalyseTool 
    {
        None,           //You can collect/analyse without any special tool
        Interrogation,  //requires a dialogue with an NPC
        Swab,           //minigame where you match the level in a testtube with a target. 
        Forceps,        //minigame based on the surgery board game. you control a shaky hand in order to pick out the sample.
        Tape            //minigame where you need to match the image to a databank e.g. checking if the fingerprint matches any in the database
    }

    [CreateAssetMenu(fileName = "Clue", menuName = "Case Data/Clue", order = 1)]
    public class Clue : CaseData
    {
        /// <summary>
        /// unique id of owning CaseElement
        /// </summary>
        [ReadOnly]
        public string elementId;

        public bool isFalseEvidence;
        public IdentifyTool indentifyTool;
        public AnalyseTool analyseTool;
    }

   
}
