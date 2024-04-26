using CGD.Networking;
using Photon.Pun;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace CGD.Economy
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] ScoreSettings settings;

        private Dictionary<int, List<ScoreType>> playerScores = new Dictionary<int, List<ScoreType>>();

        #region PUN
        public void OnEnable()
        {
            NetworkEvents.OnAllPlayersLoaded += OnAllPlayersLoaded;
            NetworkEvents.OnPlayerScoredPoints += AddScore;
            NetworkEvents.OnPlayerSubmittedClue += OnPlayerSubmittedClue;
            NetworkEvents.OnPlayerSolvedCase += OnPlayerSolvedCase;
            MiniGameManager.OnMiniGameFinished += OnMiniGameFinished;
        }

        public void OnDisable()
        {
            NetworkEvents.OnAllPlayersLoaded -= OnAllPlayersLoaded;
            NetworkEvents.OnPlayerScoredPoints -= AddScore;
            NetworkEvents.OnPlayerSubmittedClue -= OnPlayerSubmittedClue;
            NetworkEvents.OnPlayerSolvedCase -= OnPlayerSolvedCase;
            MiniGameManager.OnMiniGameFinished -= OnMiniGameFinished;
        }

        private void OnAllPlayersLoaded(double networkTime)
        {
            InitialisePlayerList();
        }

        #endregion

        private void InitialisePlayerList() 
        {
            foreach(var player in PhotonNetwork.PlayerList) 
            {
                playerScores.Add(player.ActorNumber, new List<ScoreType>());
            }
        }

        private void AddScore(int actorNumber, int scoreType) 
        {
            if (playerScores.ContainsKey(actorNumber)) 
            {
                playerScores[actorNumber].Add((ScoreType)scoreType);
            }
            else 
            {
                playerScores.Add(actorNumber , new List<ScoreType>() { (ScoreType)scoreType});
            }

            if(actorNumber == PhotonNetwork.LocalPlayer.ActorNumber) { SendData((ScoreType)scoreType); }
        }

        private void SendData(ScoreType scoreType) 
        {
            //update database from here.
            int points = settings.GetScoreValue(scoreType);
        }


        #region Callbacks
        private void OnMiniGameFinished(string clue, bool won) 
        {
            NetworkEvents.RaiseEvent_PlayerScoredPoints(PhotonNetwork.LocalPlayer.ActorNumber,
                won ? (int)ScoreType.MiniGamePassed : (int)ScoreType.MiniGameFail);
        }
        private void OnPlayerSubmittedClue(string id, int actorNumber, bool analysed)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                NetworkEvents.RaiseEvent_PlayerScoredPoints(actorNumber, (int)ScoreType.ClueSubmitted);
            }
        }
        private void OnPlayerSolvedCase(int actorNumber, bool solved)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                NetworkEvents.RaiseEvent_PlayerScoredPoints(actorNumber,
                solved ? (int)ScoreType.CaseSolveSuccess : (int)ScoreType.CaseSolveFail);
            }
        }
        #endregion

        /*
          public enum ScoreType 
    {
        MiniGamePenalty,
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
         */
    }
}