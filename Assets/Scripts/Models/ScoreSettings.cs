using UnityEngine;

namespace CGD.Economy
{
    public enum ScoreType 
    {
        MiniGamePassed,
        MiniGameFail,
        MiniGamePenalty,
        ClueSubmitted,
        SupportClueSubmitted,
        ContradictClueSubmitted,
        NarrowingClueSubmitted,
        BroadClueSubmitted,
        LeadClueSubmitted,
        ConfirmClueSubmitted,
        ShareClueLocation,
        FlaseClueSubmitted,
        PlantedCluePickedUp,
        PlantedClueSubmitted,
        RedHandedPenalty,
        RedHandedBonus,
        FalseClueRevoked,
        RealClueRevoked,
        StealEvidenceSuccess,
        DroppedClueSubmitted,
        TamperingSuccess,
        TamperClueCollectFail,
        LightsOut,
        LightsOutMiniGameFail,
        Barricade,
        CorrectAccusationInitiator,
        CorrectAccusationSupporters,
        CorrectAccusationDeceiver,
        IncorrectAccusationInitiator,
        IncorrectAccusationSupporters,
        IncorrectAccusationDeceiver,
        IncorrectAccusationAccused,
        CaseSolveSuccess,
        CaseSolveFail,
        FalseEvidenceIdentified
    }


    [CreateAssetMenu(fileName = "ScoreSettings", menuName = "Data/Scoring", order = 5)]
    public class ScoreSettings : ScriptableObject
    {
        public int[] scoreValues;

        public int GetScoreValue(ScoreType scoreType) 
        {
            return scoreValues[(int)scoreType];
        }
    }
}