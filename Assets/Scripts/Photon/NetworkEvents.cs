using CGD.Events;
using DG.Tweening.Plugins;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGD.Networking
{
    /// <summary>
    /// Encapsulates the RaiseEvents to ensure customdata formatting is maintained.
    /// </summary>
    public class NetworkEvents : Singleton<NetworkEvents>, IOnEventCallback
    {
        private static RaiseEventOptions defaultOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        private static RaiseEventOptions masterOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };

        #region PunEvent Bytes
        public const byte LoadScene = 1;
        public const byte PlayerLoaded = 2;
        public const byte AllPlayersLoaded = 3;
        public const byte GameStarted = 4;
        public const byte GameMeetingFinished = 5;
        public const byte PlayerSubmittedClue = 6;
        public const byte PlayerSharedTeamClue = 7;
        public const byte NextPlayersTurn = 8;
        public const byte PlayerSkippedTurn = 9;
        public const byte PlayerSubmittedSolution = 10;
        public const byte PlayerSolvedCase = 11;
        #endregion

        #region Delegates
        public delegate void ClueDelegate(string id, int actor, bool analysed);
        public delegate void NetworkActorDelegate(int actor, double networkTime);
        public delegate void SolutionDelegate(int actor, string[] items);
        public delegate void SolvedCaseDelegate(int actor, bool solved);

        public static IntDelegate OnLoadScene;
        public static Action OnPlayerLoaded;
        public static DoubleDelegate OnAllPlayersLoaded;
        public static Action OnGameStarted;
        public static DoubleDelegate OnGameMeetingFinished;
        public static ClueDelegate OnPlayerSubmittedClue;
        public static ClueDelegate OnPlayerSharedTeamClue;
        public static NetworkActorDelegate OnNextPlayersTurn;
        public static IntDelegate OnPlayerSkippedTurn;
        public static SolutionDelegate OnPlayerSubmittedSolution;
        public static SolvedCaseDelegate OnPlayerSolvedCase;
        #endregion

        #region Callbacks
        public void OnEvent(EventData photonEvent)
        {
            var eventCode = photonEvent.Code;

            if (eventCode == LoadScene) {
                OnLoadScene?.Invoke((int)photonEvent.CustomData);
            }
            if (eventCode == PlayerLoaded) {
                OnPlayerLoaded?.Invoke();
            }
            if (eventCode == AllPlayersLoaded) {
                OnAllPlayersLoaded?.Invoke((double)photonEvent.CustomData);
            }
            if (eventCode == GameStarted) {
                OnGameStarted?.Invoke();
            }
            if (eventCode == GameMeetingFinished) {
                OnGameMeetingFinished?.Invoke((double)photonEvent.CustomData);
            }
            if (eventCode == PlayerSubmittedClue) {
                var data = (object[])photonEvent.CustomData;
                OnPlayerSubmittedClue?.Invoke((string)data[0], (int)data[1], (bool)data[2]);
            }
            if (eventCode == PlayerSharedTeamClue)
            {
                var data = (object[])photonEvent.CustomData;
                OnPlayerSharedTeamClue?.Invoke((string)data[0], (int)data[1], (bool)data[2]);
            }
            if (eventCode == NextPlayersTurn) 
            {
                var data = (object[])photonEvent.CustomData;
                OnNextPlayersTurn?.Invoke((int)data[0], (double)data[1]);
            }
            if (eventCode == PlayerSkippedTurn)
            {
                OnPlayerSkippedTurn.Invoke((int)photonEvent.CustomData);
            }
            if (eventCode == PlayerSubmittedSolution)
            {
                var data = (object[])photonEvent.CustomData;
                OnPlayerSubmittedSolution.Invoke((int)data[0], (string[])data[1]);
            }
            if (eventCode == PlayerSolvedCase) 
            {
                var data = (object[])photonEvent.CustomData;
                OnPlayerSolvedCase.Invoke((int)data[0], (bool)data[1]);
            }
        }
        #endregion

        #region Static RaiseEvent Functions
        public static void RaiseEvent_LoadScene(int sceneIndex) 
        {
            RaiseEvent(LoadScene, sceneIndex);
        }
        public static void RaiseEvent_PlayerLoaded() 
        { 
            RaiseEvent(PlayerLoaded, null, masterOptions); 
        }
        public static void RaiseEvent_AllPlayersLoaded(double networkTime)
        {
            RaiseEvent(AllPlayersLoaded, networkTime);
        }
        public static void RaiseEvent_GameStarted() 
        {
            RaiseEvent(GameStarted, null); 
        }
        public static void RaiseEvent_GameMeetingFinished(double networkTime) 
        { 
            RaiseEvent(GameMeetingFinished, networkTime); 
        }
        public static void RaiseEvent_PlayerSubmittedClue(string clueId, int actorNumber, bool analysed)
        {
            RaiseEvent(PlayerSubmittedClue, new object[] { clueId, actorNumber, analysed });
        }
        public static void RaiseEvent_PlayerSharedClue(string clueId, int actorNumber, bool analysed)
        {
            RaiseEvent(PlayerSharedTeamClue, new object[] { clueId, actorNumber, analysed });
        }
        public static void RaiseEvent_NextPlayersTurn(int actorNumber, double networkTime)
        {
            RaiseEvent(NextPlayersTurn, new object[] { actorNumber, networkTime });
        }
        public static void RaiseEvent_PlayerSkippedTurn(int actorNumber)
        {
            RaiseEvent(PlayerSkippedTurn, actorNumber);
        }
        public static void RaiseEvent_PlayerSubmittedSolution(int actorNumber, string[] itemNames)
        {
            RaiseEvent(PlayerSubmittedSolution, new object[] { actorNumber, itemNames });
        }
        public static void RaiseEvent_PlayerSolvedCase(int actorNumber, bool solvedCase)
        {
            RaiseEvent(PlayerSolvedCase, new object[] { actorNumber, solvedCase });
        }
        private static void RaiseEvent(byte code, object data, RaiseEventOptions options = null) 
        {
            PhotonNetwork.RaiseEvent(code, data, options == null ? defaultOptions : options, SendOptions.SendReliable);
        }
        #endregion
    }
}
