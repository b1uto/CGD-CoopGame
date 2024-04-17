using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGD.Economy
{
    public class ScoreController : MonoBehaviour, IOnEventCallback
    {
        private Dictionary<int, List<ScoreType>> playerScores = new Dictionary<int, List<ScoreType>>();

        #region PUN
        public void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }

        public void OnEvent(EventData photonEvent)
        {
            var eventCode = photonEvent.Code;

            if(eventCode == GameSettings.PunAllPlayersLoaded) 
            {
                InitialisePlayerList();
            }
            /* public enum ScoreType 
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
            */


        }

        #endregion

        private void InitialisePlayerList() 
        {
            foreach(var player in PhotonNetwork.PlayerList) 
            {
                playerScores.Add(player.ActorNumber, new List<ScoreType>());
            }
        }

    }
}