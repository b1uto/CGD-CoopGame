using UnityEngine;

namespace CGD.Case
{
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

    [CreateAssetMenu(fileName = "Clue", menuName = "Case Data/Clue", order = 2)]
    public class Clue : ScriptableObject
    {
        [ReadOnly]
        public string id;

        public ClueStatus status;
        public ClueType type;
        public ClueMiniGame miniGame;

        [MaxLength(68)]
        public string shortDescription;
        [MaxLength(68)]
        public string analysedDescription;
        [TextArea(3,10)]
        public string fullDescription;

        public Sprite icon;
    }

   
}
