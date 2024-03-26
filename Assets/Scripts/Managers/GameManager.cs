using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;
using TMPro;
using System.Collections.Generic;
using UnityEngine.InputSystem;

namespace CGD
{
    public enum GameState
    {
        Loading = 0,
        Countdown = 1,
        Start = 2,
        Meeting = 3
    }

    public class GameManager : SingletonPunCallbacks<GameManager>, IOnEventCallback
    {
        #region Delegates
        public delegate void GameStateCallback(GameState state);
        public static event GameStateCallback OnGameStateChanged;

        public delegate void UpdateDelegate();
        public static UpdateDelegate OnResumeGame;
        #endregion

        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private BoardRoundManager boardManager;
        [SerializeField] private GameSettings gameSettings;

        private GameState _gameState;
        private PlayerManager localPlayerManager;

        private int loadedPlayers = 0;
        private int currentRound = 0;

        #region Properties
        public GameSettings GameSettings { get { return gameSettings; } }
        public BoardRoundManager BoardRoundManager { get { return boardManager; } }
        public PlayerManager LocalPlayerManager { get { return localPlayerManager; } }
        public GameState GameState
        {
            get
            {
                return _gameState;
            }
            private set
            {
                _gameState = value;
                OnGameStateChanged?.Invoke(value);
            }
        }
        public bool IsVoteAvailable 
        { 
            get 
            { 
                return currentRound >= gameSettings.MinNumOfRounds; 
            } 
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            //TODO pass through custom game settings
            // var filePath = System.IO.Path.Combine("Data", "Settings");
            //gameSettings = Resources.Load<GameSettings>(System.IO.Path.Combine(filePath, "GameSettings"));

            OnGameStateChanged += GameStateChangedCallback;

            if (PhotonNetwork.CurrentRoom != null)
                InstantiateNewPlayer();
            else
                PhotonNetwork.JoinRoom(PlayerPrefs.GetString(RoomProperties.RoomName));


            if (PhotonNetwork.IsMasterClient)
            {
                GenerateRoom();
            }

            //TODO Eventually put into coroutine
            RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }; // You would have to set the Receivers to All in order to receive this event on the local client as well
            PhotonNetwork.RaiseEvent(GameSettings.PunPlayerLoaded, null, raiseEventOptions, SendOptions.SendReliable);
        }

        private void OnDestroy() 
        {
            OnGameStateChanged -= GameStateChangedCallback;
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

            SceneLoader.RequestLoadScene?.Invoke(0, false);
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

        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            if (PhotonNetwork.IsMasterClient && eventCode == GameSettings.PunPlayerLoaded)
            {
                loadedPlayers++;

                if (loadedPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

                    PhotonNetwork.RaiseEvent(GameSettings.PunAllPlayersLoaded, PhotonNetwork.Time, raiseEventOptions, SendOptions.SendReliable);
                }
            }

            if (eventCode == GameSettings.PunAllPlayersLoaded)
            {
                StartGame((double)photonEvent.CustomData);
            }

            if(eventCode == GameSettings.GameMeetingFinished) 
            {
                OnMeetingFinished((double)photonEvent.CustomData);
            }
        }
        #endregion

        #region Private Methods
        private void StartGame(double networkTime) 
        {
            if (GameState == GameState.Loading)
            {
                gameSettings.SetGameTimes(networkTime);
                GameState = GameState.Countdown;
                StartCoroutine(DelayStartGame());   
            }
        }
        public void OnMeetingFinished(double networkTime)
        {
            gameSettings.UpdateRoundTime(networkTime);
            GameState = GameState.Start;
            StartCoroutine(DelayMeeting());
        }
        IEnumerator DelayStartGame() 
        {
            var delay = GameSettings.GameStartTime - PhotonNetwork.Time;
            yield return new WaitForSecondsRealtime((float)delay);
            GameState = GameState.Start;
            StartCoroutine(DelayMeeting());
        }

        IEnumerator DelayMeeting()
        {
            var delay = GameSettings.RoundEndTime - PhotonNetwork.Time;
            yield return new WaitForSecondsRealtime((float)delay);
            GameState = GameState.Meeting;
        }

        private void GameStateChangedCallback(GameState state)
        {
            switch (state) 
            {
                case GameState.Start:
                    currentRound++;
                    break;
            }
        }
        #endregion


        #region Public Methods
        public void SetLocalPlayer(PlayerManager playerManager) 
        {
            localPlayerManager = playerManager;
        }
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
            //var pos = Random.insideUnitCircle * Random.Range(1, 5);

            //PhotonNetwork.Instantiate(System.IO.Path.Combine("Items", "Torch"), new Vector3(pos.x, 1, pos.y), Quaternion.identity);
        }


        #endregion



    }
}