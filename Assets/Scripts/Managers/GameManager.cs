using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;
using CGD;
using System.IO;
using ExitGames.Client.Photon;

namespace CGD
{
    public class GameManager : SingletonPunCallbacks<GameManager>, IOnEventCallback
    {
        [SerializeField] private GameObject playerPrefab;

        private int loadedPlayers = 0;

        private void Start()
        {
            if (PhotonNetwork.CurrentRoom != null)
                InstantiateNewPlayer();
            else
                PhotonNetwork.JoinRoom(PlayerPrefs.GetString(RoomProperties.RoomKey));


            if (PhotonNetwork.IsMasterClient)
            {
                GenerateRoom();
            }

            //Eventually put into coroutine

            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(GameProperties.PlayerLoadedEvent, null, raiseEventOptions, SendOptions.SendReliable);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.P)) { LeaveRoom(); }
        }

        #region Photon Callbacks
        public override void OnLeftRoom()
        {
#if DEBUGGING
            Debug.Log("left room. returning to main menu");
#endif
            SceneManager.LoadScene(0);
        }
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
#if DEBUGGING
            DebugCanvas.Instance.AddConsoleLog("Player Joined: " + newPlayer.NickName);
#endif
        }
        public override void OnJoinedRoom()
        {
#if DEBUGGING
            Debug.Log("Late Joiner, Instantiate Player Character");
#endif
            InstantiateNewPlayer();
        }
        #endregion

        #region Public Methods
        public void LeaveRoom() => PhotonNetwork.LeaveRoom();
        #endregion

        #region Room Generation
        private void InstantiateNewPlayer()
        {
            if (PlayerManager.LocalPlayerInstance == null)
            {
#if DEBUGGING
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
#endif
                var pos = Random.insideUnitCircle * Random.Range(1, 10);

                PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(pos.x, 0, pos.y)
                    , Quaternion.identity, 0);
            }
            else
            {
#if DEBUGGING
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
#endif
            }
        }

        private void GenerateRoom()
        {
            //Clues should spawn first.
            SpawnClues();
            SpawnTools();
        }

        private void SpawnClues()
        {

        }

        private void SpawnTools()
        {
            var pos = Random.insideUnitCircle * Random.Range(1, 5);

            PhotonNetwork.Instantiate(Path.Combine("Items", "Torch"), new Vector3(pos.x, 1, pos.y), Quaternion.identity);
        }


        #endregion

        #region Interaction
        public void ClueCollected(int playerViewId, int clueId)
        {

        }

        public void OnEvent(EventData photonEvent)
        {
            if (!PhotonNetwork.IsMasterClient)
                return;

            byte eventCode = photonEvent.Code;

            if (eventCode == GameProperties.PlayerLoadedEvent)
            {
                loadedPlayers++;

                if(loadedPlayers >= PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                    PhotonNetwork.RaiseEvent(GameProperties.AllPlayersLoadedEvent, null, raiseEventOptions, SendOptions.SendReliable);
                }
            }
        }

        #endregion

    }
}