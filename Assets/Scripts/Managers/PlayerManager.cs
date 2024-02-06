using System.Collections.Generic;
using System.IO;
using Fusion;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace CGD
{
    public class PlayerManager : MonoBehaviourPunCallbacks
    {
        public static GameObject LocalPlayerInstance;

        private void Awake()
        {
            var tmp = GetComponentInChildren<TextMeshPro>();
            if (tmp && photonView)
            {
                tmp.text = photonView.Owner.NickName;
            }
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (photonView == null)
            {
                LocalPlayerInstance = this.gameObject;
                return;
            }

            if (photonView.IsMine)
            {
                LocalPlayerInstance = this.gameObject;

                var viewId = PhotonNetwork.Instantiate(Path.Combine("Models", GameManager.GetRandomModelName()),
                    transform.position, Quaternion.identity).GetPhotonView().ViewID;

                photonView.RPC("InitialiseModel", RpcTarget.AllBuffered, viewId);
            }
        }

        [PunRPC]
        private void InitialiseModel(int viewId)
        {
            var model = PhotonView.Find(viewId).gameObject;
            if (model)
            {
                model.transform.SetParent(transform);
                model.transform.localPosition = Vector3.zero;
                GetComponent<PlayerAnimController>().animator = model.GetComponent<Animator>();
            }
        }
    }
}