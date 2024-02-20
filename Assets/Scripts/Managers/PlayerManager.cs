using System.Collections.Generic;
using System.IO;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace CGD
{
    public class PlayerManager : MonoBehaviourPunCallbacks
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


        private List<int> collectedClues = new List<int>();


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
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (photonView.IsMine)
            {
                LocalPlayerInstance = this.gameObject;

                if (MenuManager.Instance != null)
                    MenuManager.Instance.OnMenuClosed.AddListener(MenuClosed);



                var modelPath = Path.Combine("Models", GetRandomModelName());
                photonView.RPC(nameof(InstantiateCharacter), RpcTarget.AllBuffered, photonView.ViewID, modelPath);
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
        

        public void GetClue(int clueId) 
        {
            collectedClues.Add(clueId);
            if(photonView.IsMine) 
            {
                MenuManager.Instance.OpenMenu("clue");
                GetComponent<PlayerInputHandler>().DisablePlayerInput();
            }
        }

        public void MenuClosed(string menu) 
        {
            if (photonView.IsMine && menu == "clue")
            {
                GetComponent<PlayerInputHandler>().EnablePlayerInput();
            }
        }

    }
}