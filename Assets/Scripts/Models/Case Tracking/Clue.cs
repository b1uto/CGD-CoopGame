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
    public enum ClueType
    {
        HiddenStain = 0,
        HiddenWriting = 1,
        Prints = 2,
        Item = 3,
        Residue = 4,
        PersonOfInterest = 5
    }
    public enum ClueMiniGame
    {
        Automatic = 0,
        MatchImage = 1,
        Interrogation = 2,
        MatchVolume = 3,
        Oscilloscope = 4

    }

    public class Clue : CaseData
    {
        /// <summary>
        /// unique id of owning CaseElement
        /// </summary>
        [ReadOnly]
        public string elementId;

        public ClueType type;
        public ClueMiniGame miniGame;

    }

   
}
