using UnityEngine;

namespace CGD.Economy
{
    public enum ScoreType 
    {
        ClueCollected,
        ClueAnalysed,
        ClueSubmitted,
        ClueRevoked,
        AbilityUsedSuccess,
        AbilityUsedFail,
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