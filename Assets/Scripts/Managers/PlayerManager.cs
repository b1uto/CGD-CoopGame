using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerManager : MonoBehaviourPunCallbacks
{
    public static GameObject LocalPlayerInstance;


    private void Awake()
    {
        if (photonView.IsMine)
        {
            LocalPlayerInstance = this.gameObject;
        }
        var tmp = GetComponentInChildren<TextMeshPro>();
        if (tmp)
        {
            tmp.text = photonView.Owner.NickName;
        }

        DontDestroyOnLoad(gameObject);
    }


}
