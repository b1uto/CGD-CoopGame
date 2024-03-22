using CGD.Case;
using CGD.Events;
using CGD.Extensions;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;

namespace CGD
{
    public class BoardRoundManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public delegate void ClueSubmitDelegate(string id, int actorNumber, bool analysed);

        public static event ClueSubmitDelegate OnClueSubmitted;
        public static event IntDelegate OnNextPlayerTurn;
        public static event IntDelegate OnPlayerSkippedTurn;


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


        private RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        private Coroutine turnTimerCoroutine;

        public Player[] PlayerList { get { return playerList; } }
        private GameManager GM { get { return GameManager.Instance; } }


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
                object[] data = (object[])photonEvent.CustomData;

                OnClueSubmitted?.Invoke((string)data[0], (int)data[1], (bool)data[2]);
                StopAllCoroutines();
                Invoke("NextPlayer", 3);
            }

            if(eventCode == GameSettings.OnPlayerSkippedTurn)
            {
                OnPlayerSkippedTurn?.Invoke((int)photonEvent.CustomData);
                StopAllCoroutines();
                Invoke("NextPlayer", 3);
            }

            if(eventCode == GameSettings.OnNextPlayersTurn) 
            {
                object[] data = (object[])photonEvent.CustomData;

                GM.GameSettings.SetTurnEndTime((double)data[1]);
                OnNextPlayerTurn?.Invoke((int)data[0]); 

                if (PhotonNetwork.IsMasterClient)
                    CoroutineUtilities.StartExclusiveCoroutine(TurnTimer(), ref turnTimerCoroutine, this);
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
            activePlayer = 0;
            NextPlayer();
        }

        private void FinishCardRound()
        {
            PhotonNetwork.RaiseEvent(GameSettings.GameMeetingFinished, PhotonNetwork.Time, raiseEventOptions, SendOptions.SendReliable);
        }
        #endregion

        #region Turn Manager
        private void NextPlayer()
        {
            if (!PhotonNetwork.IsMasterClient) 
                return;

            if (activePlayer >= playerList.Length)
            {
                FinishCardRound();
            }
            else
            {
                object[] eventData = new object[] { playerList[activePlayer++].ActorNumber, PhotonNetwork.Time } ;

                PhotonNetwork.RaiseEvent(GameSettings.OnNextPlayersTurn, eventData, raiseEventOptions, SendOptions.SendReliable);


            }
        }

        public void Btn_SkipTurn() 
        {
#if DEBUGGING
            DebugCanvas.Instance.AddConsoleLog("<color=yellow>Skipping Turn</color>");
#endif

            PhotonNetwork.RaiseEvent(GameSettings.OnPlayerSkippedTurn,
                PhotonNetwork.LocalPlayer.ActorNumber, raiseEventOptions, SendOptions.SendReliable);
        }

        IEnumerator TurnTimer() 
        {
            var delay = GM.GameSettings.TurnEndTime - PhotonNetwork.Time;
            yield return new WaitForSecondsRealtime((float)delay);

            NextPlayer();
        }

        //IEnumerator DelayStartCardRound(float delay = 3) 
        //{
        //    yield return new WaitForSecondsRealtime(delay);
        //    //NextPlayer();
        //}

        #endregion

    }
}