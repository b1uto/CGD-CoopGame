using System.Collections.Generic;
using CGD.Case;
using CGD.Input;
using CGD.Networking;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace CGD.Gameplay
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        public static GameObject LocalPlayerInstance;

        private Dictionary<string, ClueInfo> clues = new Dictionary<string, ClueInfo>();
        private CGD.Input.PlayerInputHandler inputHandler;

        public ref Dictionary<string, ClueInfo> Clues { get { return ref clues; } }

        private void Awake()
        {
            var tmp = GetComponentInChildren<TextMeshPro>();
            if (tmp && photonView)
            {
                tmp.text = photonView.Owner.NickName;
                tmp.gameObject.SetActive(!photonView.IsMine);
            }

            if (photonView.IsMine)
            {
                inputHandler = gameObject.AddComponent<CGD.Input.PlayerInputHandler>();
                GameManagerEvents.OnGameStateChanged += OnGameStateChanged;
                GameManagerEvents.OnResumeGame += OnResumeGame;
                NetworkEvents.OnPlayerSubmittedClue += OnPlayerSubmittedClue;
                NetworkEvents.OnPlayerSharedTeamClue += OnPlayerSubmittedClue;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (photonView.IsMine)
            {
                GameManagerEvents.OnGameStateChanged -= OnGameStateChanged;
                GameManagerEvents.OnResumeGame -= OnResumeGame;
                NetworkEvents.OnPlayerSubmittedClue -= OnPlayerSubmittedClue;
                NetworkEvents.OnPlayerSharedTeamClue -= OnPlayerSubmittedClue;
            }
        }


        private void Start()
        {
            if (photonView.IsMine)
            {
                LocalPlayerInstance = this.gameObject;

                string modelPath = "";

                if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(PlayerProperties.Model, out object Model))
                {
                    modelPath = System.IO.Path.Combine("Models", ItemCollection.GetModelName((int)Model));
                }
                else
                {
                    modelPath = System.IO.Path.Combine("Models", ItemCollection.GetRandomModelName());
                }

                photonView.RPC(nameof(InstantiateCharacter), RpcTarget.AllBuffered, photonView.ViewID, modelPath);
                GameManager.Instance.SetLocalPlayer(this);
            }
        }

        [PunRPC]
        private void InstantiateCharacter(int ownerViewID, string modelPath)
        {
            var owner = PhotonView.Find(ownerViewID);
            if (owner && !string.IsNullOrEmpty(modelPath) && TryGetComponent(out PlayerAnimController animController))
            {
                animController.InitialiseAnimController(owner, modelPath);

                //var model = Resources.Load<GameObject>(modelPath);
                //var modelTransform = Instantiate(model, owner.transform).transform;
                //modelTransform.localPosition = Vector3.zero;
                //modelTransform.localRotation = Quaternion.identity;

                //GetComponent<PlayerAnimController>().animator = modelTransform.GetComponent<Animator>();
            }
            else 
            {
#if DEBUGGING
                Debug.LogError("Error instantiating character: Make sure PhotonView exists and model path is valid");
#endif
            }
        }



        public void SubmitClue(string id)
        {
            if (clues.TryGetValue(id, out var info))
            {
                NetworkEvents.RaiseEvent_PlayerSubmittedClue(id, PhotonNetwork.LocalPlayer.ActorNumber, info.status == ClueStatus.Analysed);
            }
        }

        public void ShareClueWithTeam(string id)
        {
            if (clues.TryGetValue(id, out var info))
            {
                NetworkEvents.RaiseEvent_PlayerSharedClue(id, PhotonNetwork.LocalPlayer.ActorNumber, info.status == ClueStatus.Analysed);
            }
        }

        public void CollectClue(string id) 
        {
            if (ItemCollection.Instance.TryGetCaseData(id, out Clue clue))
            {
                clues.Add(id, new ClueInfo(false, ClueStatus.Collected));

                if (photonView.IsMine)
                {
                    GameMenuManager.Instance.OpenCluePanel(clue);
                    InputManager.Instance.SetActiveMap(GameContext.UI);

                    if(GameManager.Instance.GameMode == GameMode.Team) 
                        ShareClueWithTeam(id);
                }
            }
        }

        public void OnResumeGame() => InputManager.Instance.SetActiveMap(GameContext.Game);

        private void OnGameStateChanged(GameState state) 
        {
            switch(state) 
            {
                case GameState.Countdown:
                    BoardRoundManager.Instance.PlaceActor(PhotonNetwork.LocalPlayer, gameObject);
                    GetComponent<PlayerController>().SmoothLookAt(BoardRoundManager.Instance.Target.position);
                    break;
                case GameState.Start:
                    //inputHandler.EnablePlayerInput(); 
                    break;
                case GameState.Meeting:
                    //inputHandler.DisablePlayerInput();
                    BoardRoundManager.Instance.PlaceActor(PhotonNetwork.LocalPlayer, gameObject);
                    GetComponent<PlayerController>().SmoothLookAt(BoardRoundManager.Instance.Target.position);
                    break;
            }
        }



        #region RaiseEvent Callback 

        private void OnPlayerSubmittedClue(string id, int actorNumber, bool analysed) 
        {
            UpdateClue(id, new ClueInfo(true, analysed ? ClueStatus.Analysed : ClueStatus.Collected));
        }

        //public void OnEvent(EventData photonEvent)
        //{
        //    byte eventCode = photonEvent.Code;

        //    if (eventCode == GameSettings.PlayerSubmittedClue) 
        //    {
        //        var data = (object[])photonEvent.CustomData;

        //        var clue = (string)data[0];
        //        var clueInfo = new ClueInfo(true, (bool)data[2] ? ClueStatus.Analysed : ClueStatus.Collected);
                
        //        UpdateClue(clue, clueInfo);
        //    }
        //    if (eventCode == GameSettings.PlayerSharedClue)
        //    {
        //        var data = (object[])photonEvent.CustomData;

        //        var clue = (string)data[0];
        //        var clueInfo = new ClueInfo(true, (bool)data[2] ? ClueStatus.Analysed : ClueStatus.Collected);

        //        UpdateClue(clue, clueInfo);
        //    }
        //}

        private void UpdateClue(string clue, ClueInfo clueInfo)
        {
            if (clues.ContainsKey(clue))
            {
                clues[clue] = clueInfo;
            }
            else
            {
                clues.Add(clue, clueInfo);
            }
        }
        #endregion


        #region DEBUG

        //private void AssignRandomClues()
        //{
        //    if (PhotonNetwork.IsMasterClient)
        //    {
        //        var keys = ItemCollection.Instance.GetClueKeys();

        //        int keysPerClient = keys.Length / PhotonNetwork.PlayerList.Length;

        //        int index = 0;

        //        foreach (var player in PhotonNetwork.PlayerList)
        //        {
        //            if (index >= keys.Length)
        //                break;

        //            var cluesToAdd = new List<string>();

        //            for(int i = 0; i < keysPerClient; i++) 
        //            {
        //                cluesToAdd.Add(keys[index++]);
        //            }

        //            AddDebugClues(cluesToAdd.ToArray(), player.ActorNumber);
        //        }
        //    }
        //}

        //public void AddDebugClues(string[] cluesToAdd, int actorNumber) 
        //{
        //    photonView.RPC(nameof(GiveClue), RpcTarget.All, actorNumber, cluesToAdd);
        //}

        //[PunRPC]
        //private void GiveClue(int actorNumber, string[] cluesToAdd)
        //{
        //    if (actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        //    {
        //        foreach (var clue in cluesToAdd)
        //        {
        //            clues.Add(clue, new ClueInfo(false, ClueStatus.Collected));
        //        }
        //    }
        //}

        #endregion

    }
}