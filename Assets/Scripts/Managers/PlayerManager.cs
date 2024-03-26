using System.Collections.Generic;
using System.IO;
using System.Xml.Schema;
using CGD.Case;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CGD
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public static GameObject LocalPlayerInstance;

      
        public ref Dictionary<string, ClueInfo> Clues { get { return ref clues; } }

        private Dictionary<string, ClueInfo> clues = new Dictionary<string, ClueInfo>();


        private void Awake()
        {
            var tmp = GetComponentInChildren<TextMeshPro>();
            if (tmp && photonView)
            {
                tmp.text = photonView.Owner.NickName;
            }

            if (photonView.IsMine)
            {
                gameObject.AddComponent<PlayerInputHandler>();
                GameManager.OnGameStateChanged += OnGameStateChanged;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void OnDestroy()
        {
            if (photonView.IsMine)
            {
                GameManager.OnGameStateChanged -= OnGameStateChanged;
            }
        }


        private void Start()
        {
            if (photonView.IsMine)
            {
                LocalPlayerInstance = this.gameObject;

                var modelPath = Path.Combine("Models", ItemCollection.GetRandomModelName());
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
                object[] data = new object[] { id, PhotonNetwork.LocalPlayer.ActorNumber, info.status == ClueStatus.Analysed};

                RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
                PhotonNetwork.RaiseEvent(GameSettings.PlayerSubmittedClue, data, raiseEventOptions, SendOptions.SendReliable);
            }
        }

        public void CollectClue(string id) 
        {
            if (ItemCollection.Instance.TryGetCaseData(id, out Clue clue))
            {
                clues.Add(id, new ClueInfo(false, ClueStatus.Collected));

                if (photonView.IsMine)
                {
                    MenuManager.Instance.OpenMenu("clue");
                    GetComponent<PlayerInputHandler>().DisablePlayerInput();
                }
            }
        }

        private void OnGameStateChanged(GameState state) 
        {
            switch(state) 
            {
                case GameState.Countdown:
                    GameManager.Instance.BoardRoundManager.PlaceActor(PhotonNetwork.LocalPlayer, gameObject);
                    GetComponent<PlayerController>().SmoothLookAt(GameManager.Instance.BoardRoundManager.Target.position);
                    break;
                case GameState.Start:
                    GetComponent<PlayerInputHandler>().EnablePlayerInput(); 
                    break;
                case GameState.Meeting:
                    GetComponent<PlayerInputHandler>().DisablePlayerInput();
                    GameManager.Instance.BoardRoundManager.PlaceActor(PhotonNetwork.LocalPlayer, gameObject);
                    GetComponent<PlayerController>().SmoothLookAt(GameManager.Instance.BoardRoundManager.Target.position);
                    break;
            }
        }


        #region IOnEventCallback 
        public void OnEvent(EventData photonEvent)
        {
            byte eventCode = photonEvent.Code;

            if (eventCode == GameSettings.PlayerSubmittedClue) 
            {
                var data = (object[])photonEvent.CustomData;

                var clue = (string)data[0];
                var clueInfo = new ClueInfo(true, (bool)data[2] ? ClueStatus.Analysed : ClueStatus.Collected);
                
                UpdateClue(clue, clueInfo); 
            }

            //if(eventCode == GameSettings.PunAllPlayersLoaded) 
            //{
            //    AssignRandomClues();
            //}
        }

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