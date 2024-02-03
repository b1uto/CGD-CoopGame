using System.IO;
using Fusion;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;

    private void Awake() 
    {
        var tmp = GetComponentInChildren<TextMeshPro>();
        if (tmp)
        {
            tmp.text = photonView.Owner.NickName;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = this.gameObject;

            var viewId = PhotonNetwork.Instantiate(Path.Combine("Models", "female01"),
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

    //public void OnPhotonInstantiate(PhotonMessageInfo info)
    //{
    //    if (photonView.Owner.ActorNumber == info.Sender.ActorNumber)
    //    {
    //        info.Sender.TagObject = gameObject;
    //        DebugCanvas.Instance.AddConsoleLog("Set Player Ref: " + info.Sender.ActorNumber);
    //    }
    //}
}
