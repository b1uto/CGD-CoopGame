using CGD.Case;
using CGD.Events;
using CGD.Extensions;
using CGD.Networking;
using DG.Tweening.Plugins;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PlayFab.ClientModels;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace CGD.Gameplay
{
    public class BoardRoundManager : SingletonPunCallbacks<BoardRoundManager>
    {
        //public delegate void ClueSubmitDelegate(string id, int actorNumber, bool analysed);

       // public static event ClueSubmitDelegate OnClueSubmitted;
       //  public static event IntDelegate OnNextPlayerTurn;
        //public static event IntDelegate OnPlayerSkippedTurn;
         public static event IntDelegate OnMoveMade;

        [SerializeField] private Transform[] playerPoints;
        [SerializeField] private Transform playerTarget;

        private Player[] playerList;

        /// <summary>
        /// index of the player with the active turn.
        /// </summary>
        private int activePlayer = -1;

        /// <summary>
        /// dictates if turns rotate clockwise/anticlockwise around the table.
        /// </summary>
        private bool clockwiseTurn = true;

        /// <summary>
        /// moves left for current player
        /// </summary>
        private int movesLeft = 0;



        private RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

        private Coroutine turnTimerCoroutine;

        public Player[] PlayerList { get { return playerList; } }
        public Transform Target { get { return playerTarget; } }
        private GameManager GM { get { return GameManager.Instance; } }


        #region Setup

        private void Start()
        {
            playerList = PhotonNetwork.PlayerList;
            GameManagerEvents.OnGameStateChanged += GameStateChanged;
        }

        private void OnDestroy()
        {
            GameManagerEvents.OnGameStateChanged -= GameStateChanged;
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
        public void SubmitSolution(int actorNumber, CaseItem[] items) 
        {
            NetworkEvents.RaiseEvent_PlayerSubmittedSolution(actorNumber, items.Select(x => x.name).ToArray());
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



        //public void OnEvent(EventData photonEvent)
        //{
        //    byte eventCode = photonEvent.Code;

        //    if (eventCode == GameSettings.PlayerSubmittedClue)
        //    {
        //        object[] data = (object[])photonEvent.CustomData;

        //        OnClueSubmitted?.Invoke((string)data[0], (int)data[1], (bool)data[2]);
        //        TurnUsed();
        //    }
        //    if (eventCode == GameSettings.PlayerSubmittedSolution)
        //    {
        //        var data = (object[])photonEvent.CustomData;
        //        OnPlayerAttemptedSolve((int)data[0], (string[])data[1]);    
        //    }

        //    if (eventCode == GameSettings.OnPlayerSkippedTurn)
        //    {
        //        OnPlayerSkippedTurn?.Invoke((int)photonEvent.CustomData);
        //        StopAllCoroutines();
        //        Invoke("NextPlayer", 3);
        //    }

        //    if(eventCode == GameSettings.OnNextPlayersTurn) 
        //    {
        //        object[] data = (object[])photonEvent.CustomData;

        //        movesLeft = GM.GameSettings.MovesPerTurn;
        //        GM.GameSettings.SetTurnEndTime((double)data[1]);
        //        OnNextPlayerTurn?.Invoke((int)data[0]); 

        //        if (PhotonNetwork.IsMasterClient)
        //            CoroutineUtilities.StartExclusiveCoroutine(TurnTimer(), ref turnTimerCoroutine, this);
        //    }
        //}


        #endregion
        #region RaiseEvent Callbacks
        private void OnPlayerSubmittedClue() { TurnUsed(); }
        public void OnPlayerSubmittedSolve(int actorNumber, string[] guessedItems)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                var items = ItemCollection.Instance.GetActiveCaseItems().Select(x => x.name).ToArray();

                bool caseSolved = guessedItems.Length == items.Length &&
                    !guessedItems.Except(items).Any() &&
                    !items.Except(guessedItems).Any();


                NetworkEvents.RaiseEvent_PlayerSolvedCase(actorNumber, caseSolved);

                if (!caseSolved)
                {
                    movesLeft = 0;
                    TurnUsed();
                }

            }
        }
        private void OnPlayerSkippedTurn(int actorNumber)
        {
            StopAllCoroutines();
            Invoke("NextPlayer", 3);
        }
        private void OnNextPlayersTurn(double networkTime, int actorNumber)
        {
            movesLeft = GM.GameSettings.MovesPerTurn;
            GM.GameSettings.SetTurnEndTime(networkTime);
            //OnNextPlayerTurn?.Invoke((int)data[0]);

            if (PhotonNetwork.IsMasterClient)
                CoroutineUtilities.StartExclusiveCoroutine(TurnTimer(), ref turnTimerCoroutine, this);
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
            NetworkEvents.RaiseEvent_GameMeetingFinished(PhotonNetwork.Time);
        }
        #endregion

        #region Turn Manager
       
        private void TurnUsed()
        {
            if (--movesLeft <= 0)
            {
                StopAllCoroutines();
                Invoke("NextPlayer", 3);
            }

            OnMoveMade?.Invoke(movesLeft);
        }
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
                NetworkEvents.RaiseEvent_NextPlayersTurn( playerList[activePlayer++].ActorNumber, PhotonNetwork.Time );
            }
        }

        public void Btn_SkipTurn() 
        {
#if DEBUGGING
            if (DebugCanvas.Instance) DebugCanvas.Instance.AddConsoleLog("<color=yellow>Skipping Turn</color>");
#endif

            NetworkEvents.RaiseEvent_PlayerSkippedTurn(PhotonNetwork.LocalPlayer.ActorNumber);
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