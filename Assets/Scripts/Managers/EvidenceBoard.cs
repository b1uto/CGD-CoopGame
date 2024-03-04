using CGD.Events;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

namespace CGD
{
    public class EvidenceBoard : MonoBehaviourPunCallbacks
    {
        public delegate void PlayerDelegate(Player player);
        public static event PlayerDelegate OnNextPlayerTurn;

        [SerializeField] private Transform[] playerPoints;

        private Player[] playerList;

        /// <summary>
        /// index of the player with the active turn.
        /// </summary>
        private int activePlayer = 0;

        /// <summary>
        /// dictates if turns rotate clockwise/anticlockwise around the table.
        /// </summary>
        private bool clockwiseTurn = true;


        private List<GameObject> communityCards = new List<GameObject>();


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
        #endregion

        #region Main
        private void GameStateChanged(GameState gameState)
        {
            if (gameState == GameState.Meeting)
                StartCardRound();
        }

        private void StartCardRound()
        {
            if (PhotonNetwork.IsMasterClient) 
            {
            
            }
        }

        private void FinishCardRound()
        {

        }
        #endregion

    }
}