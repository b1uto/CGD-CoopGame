using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine;
using System.Collections;
using CGD.Case;
using CGD.Networking;
using System.Diagnostics.Tracing;

namespace CGD.Gameplay
{

    public enum GameMode 
    {
        Standard,
        Team
    }
    public enum GameState
    {
        Loading = 0,
        Countdown = 1,
        Start = 2,
        Meeting = 3,
        Finished = 4
    }


    public static class GameManagerEvents 
    {
        #region Delegates
        public delegate void GameStateCallback(GameState state);
        public static GameStateCallback OnGameStateChanged;

        public static System.Action OnResumeGame;
        #endregion

    }

    public class GameManager : SingletonPunCallbacks<GameManager>
    {
       
        [SerializeField] private GameObject playerPrefab;
        //[SerializeField] private BoardRoundManager boardManager;
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private CaseFile activeCase;

        private GameState _gameState;
        private GameMode _gameMode;
        private PlayerManager localPlayerManager;

        private int loadedPlayers = 0;
        private int currentRound = 0;

        #region Properties
        public GameSettings GameSettings { get { return gameSettings; } }
        //public BoardRoundManager BoardRoundManager { get { return boardManager; } }
        public PlayerManager LocalPlayerManager { get { return localPlayerManager; } }
        public CaseFile ActiveCase { get { return activeCase; } }
        public GameState GameState
        {
            get
            {
                return _gameState;
            }
            private set
            {
                _gameState = value;
                GameManagerEvents.OnGameStateChanged?.Invoke(value);
            }
        }

        public GameMode GameMode 
        {
            get 
            { 
                return _gameMode;
            }
        }
        #endregion

        protected override void Awake()
        {
            base.Awake();

            //TODO pass through custom game settings
            // var filePath = System.IO.Path.Combine("Data", "Settings");
            //gameSettings = Resources.Load<GameSettings>(System.IO.Path.Combine(filePath, "GameSettings"));

            GameManagerEvents.OnGameStateChanged += GameStateChangedCallback;
            NetworkEvents.OnPlayerLoaded += OnPlayerLoaded;
            NetworkEvents.OnAllPlayersLoaded += OnAllPlayersLoaded;
            NetworkEvents.OnGameMeetingFinished += OnGameMeetingFinished;
            NetworkEvents.OnPlayerSolvedCase += OnPlayerSolvedCase;


            if (PhotonNetwork.CurrentRoom != null)
            {
                SetRoomInfo();
                InstantiateNewPlayer();
            }
            else
                PhotonNetwork.JoinRoom(PlayerPrefs.GetString(RoomProperties.RoomName));


            if (PhotonNetwork.IsMasterClient)
            {
                GenerateRoom();
            }

            NetworkEvents.RaiseEvent_PlayerLoaded(); 
        }

        private void OnDestroy() 
        {
            GameManagerEvents.OnGameStateChanged -= GameStateChangedCallback;
            NetworkEvents.OnPlayerLoaded -= OnPlayerLoaded;
            NetworkEvents.OnAllPlayersLoaded -= OnAllPlayersLoaded;
            NetworkEvents.OnGameMeetingFinished -= OnGameMeetingFinished;
            NetworkEvents.OnPlayerSolvedCase -= OnPlayerSolvedCase;
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

            SetRoomInfo();
            InstantiateNewPlayer();
        }

        //public void OnEvent(EventData photonEvent)
        //{
        //    byte eventCode = photonEvent.Code;

        //    if (PhotonNetwork.IsMasterClient && eventCode == GameSettings.PunPlayerLoaded)
        //    {
        //        loadedPlayers++;

        //        if (loadedPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
        //        {
        //            GameSettings.RE_PunAllPlayersLoaded(PhotonNetwork.Time);
        //        }
        //    }

        //    if (eventCode == GameSettings.PunAllPlayersLoaded)
        //    {
        //        StartGame((double)photonEvent.CustomData);
        //    }

        //    if(eventCode == GameSettings.GameMeetingFinished) 
        //    {
        //        OnMeetingFinished((double)photonEvent.CustomData);
        //    }

        //    if (eventCode == GameSettings.PlayerSolvedCase)
        //    {
        //        var data = (object[])photonEvent.CustomData;
        //        var actorNumber = (int)data[0];
        //        var solved = (bool)data[1];

        //        if(solved) { FinishGame(); }
        //    }
        //}
        #endregion

        #region NetworkEvents
        private void OnPlayerLoaded()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                loadedPlayers++;

                if (loadedPlayers == PhotonNetwork.CurrentRoom.PlayerCount)
                {
                    NetworkEvents.RaiseEvent_AllPlayersLoaded(PhotonNetwork.Time);
                }
            }
        }

        private void OnAllPlayersLoaded(double networkTime)
        {
            StartGame(networkTime);
        }

        private void OnGameMeetingFinished(double networkTime)
        {
            OnMeetingFinished(networkTime);
        }

        private void OnPlayerSolvedCase(int actorNumber, bool solved)
        {
            if (solved) { FinishGame(); }
        }
#endregion

#region Public Methods
public void ResumeGame() 
        {
            GameManagerEvents.OnResumeGame?.Invoke();
        }
        public void SetLocalPlayer(PlayerManager playerManager)
        {
            localPlayerManager = playerManager;
        }
        public void LeaveRoom() => PhotonNetwork.LeaveRoom();
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
        private void OnMeetingFinished(double networkTime)
        {
            gameSettings.UpdateRoundTime(networkTime);
            GameState = GameState.Start;
            StartCoroutine(DelayMeeting());
        }
        private void FinishGame() 
        {
            GameState = GameState.Finished;
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

        #endregion

        #region Room Generation
        private void SetRoomInfo() 
        {
            var roomProperties = PhotonNetwork.CurrentRoom.CustomProperties;

            if (roomProperties.TryGetValue(RoomProperties.TeamGame, out object TeamGame))
            {
                _gameMode = (bool)TeamGame ? GameMode.Team : GameMode.Standard;
            }
        }

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

            var pos = GameObject.Find("TorchSpawnPoint").transform.position;
            PhotonNetwork.Instantiate(System.IO.Path.Combine("Items", "Torch"), new Vector3(pos.x, 1, pos.y), Quaternion.identity);
        }


        #endregion



    }
}