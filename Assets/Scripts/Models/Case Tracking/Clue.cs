using UnityEngine;

namespace CGD.Case
{
    /// <summary>
    /// Used at runtime.
    /// </summary>
    public enum ClueStatus
    {
        Hidden,
        Discovered,
        Asserted,
        Analysed
    }

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


    //Note I don't know if these need to be scriptable objects. but it allows us to build out a full case in the editor.

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
