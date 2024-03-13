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

namespace CGD
{
    public class PlayerManager : MonoBehaviourPunCallbacks, IOnEventCallback
    {
        public static GameObject LocalPlayerInstance;

        /// <summary>
        /// TODO store elsewhere in Item Collection. Randomise if player has not chosen avatar.
        /// </summary>
        private readonly string[] modelNames = new string[5]
        {
        "female01",
        "female02",
        "male01",
        "male02",
        "male03"
        };

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

                var modelPath = Path.Combine("Models", GetRandomModelName());
                photonView.RPC(nameof(InstantiateCharacter), RpcTarget.AllBuffered, photonView.ViewID, modelPath);
                GameManager.Instance.SetLocalPlayer(this);
            }
        }

        [PunRPC]
        private void InstantiateCharacter(int ownerViewID, string modelPath)
        {
            var owner = PhotonView.Find(ownerViewID);
            if (owner && !string.IsNullOrEmpty(modelPath))
            {
                var model = Resources.Load<GameObject>(modelPath);
                var modelTransform = Instantiate(model, owner.transform).transform;
                modelTransform.localPosition = Vector3.zero;
                modelTransform.localRotation = Quaternion.identity;

                GetComponent<PlayerAnimController>().animator = modelTransform.GetComponent<Animator>();
            }
            else 
            {
#if DEBUGGING
                Debug.LogError("Error instantiating character: Make sure PhotonView exists and model path is valid");
#endif
            }
        }
        private string GetRandomModelName() 
        {
            return modelNames[Random.Range(0, modelNames.Length)];
        }

        public void SubmitClue(string id)
        {
            if (clues.TryGetValue(id, out var info))
            {
                object[] data = new object[] { id, (int)info.status, PhotonNetwork.LocalPlayer.ActorNumber };

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
                case GameState.Start:
                    GetComponent<PlayerInputHandler>().EnablePlayerInput(); 
                    break;
                case GameState.Meeting:
                    GetComponent<PlayerInputHandler>().DisablePlayerInput();
                    GameManager.Instance.BoardRoundManager.PlaceActor(PhotonNetwork.LocalPlayer, gameObject);
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
                var clueInfo = new ClueInfo(true, (ClueStatus)data[1]);
                
                UpdateClue(clue, clueInfo); 
            }
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

    }
}