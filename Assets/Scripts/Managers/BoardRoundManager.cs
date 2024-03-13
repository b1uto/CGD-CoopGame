using CGD.Case;
using CGD.Events;
using CGD.Extensions;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGD
{
    public class BoardRoundManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public delegate void PlayerDelegate(Player player, int index);
        public static event PlayerDelegate OnNextPlayerTurn;

        [SerializeField] private Transform[] playerPoints;

        private Player[] playerList;

        /// <summary>
        /// index of the player with the active turn.
        /// </summary>
        private int activePlayer = -1;

        /// <summary>
        /// dictates if turns rotate clockwise/anticlockwise around the table.
        /// </summary>
        private bool clockwiseTurn = true;

        private Coroutine turnTimerCoroutine;

        #region Setup
        private void Start()
        {
            playerList = PhotonNetwork.PlayerList;
            GameManager.OnGameStateChanged += GameStateChanged;
        }

        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameStateChanged;
        }
        #endregion

        #region Public Functions
        public void PlaceActor(Player player, GameObject pawn)
        {
            int index = System.Array.IndexOf(playerList, player);

            if (index >= 0 && index < playerPoints.Length)
            {
                pawn.transform.position = playerPoints[index].position;
                pawn.transform.rotation = playerPoints[index].rotation;
            }
        }

        public void SubmitClue(string id, ClueStatus status)
        {
        }
        #endregion

        #region PUN callbacks
        public override void OnLeftRoom()
        {
            playerList = PhotonNetwork.PlayerList;
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            playerList = PhotonNetwork.PlayerList;
        }
        public override void OnJoinedRoom()
        {
            playerList = PhotonNetwork.PlayerList;
        }

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            if (eventCode == GameSettings.PlayerSubmittedClue) 
            {
                var data = (object[])photonEvent.CustomData;

            }
        }
        #endregion

        #region Main
        private void GameStateChanged(GameState gameState)
        {
            if (gameState == GameState.Meeting)
                StartCardRound();
        }

        private void StartCardRound()
        {
            StartCoroutine(DelayStartCardRound());
        }

        private void FinishCardRound()
        {
            activePlayer = -1;
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(GameSettings.GameMeetingFinished, PhotonNetwork.Time, raiseEventOptions, SendOptions.SendReliable);
        }
        #endregion

        #region Turn Manager
        private void NextPlayer()
        {
            if (!PhotonNetwork.IsMasterClient) 
                return;

            activePlayer++;

            if (activePlayer >= playerList.Length)
                FinishCardRound();
            else
            {
                OnNextPlayerTurn?.Invoke(playerList[activePlayer], activePlayer);
                    
                if(PhotonNetwork.IsMasterClient)
                    CoroutineUtilities.StartExclusiveCoroutine(TurnTimer(), ref turnTimerCoroutine, this);
            }
        }

        IEnumerator TurnTimer() 
        {
            var delay = GameManager.Instance.GameSettings.TurnTime;
            yield return new WaitForSecondsRealtime((float)delay);
            NextPlayer();
        }

        IEnumerator DelayStartCardRound(float delay = 3) 
        {
            yield return new WaitForSecondsRealtime(delay);
            NextPlayer();
        }

        #endregion

    }
}