using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsClient : MonoBehaviourPunCallbacks
{
    [Serializable]
    public struct Constraints 
    {
        public bool X;
        public bool Y;
        public bool Z;
    }

    [SerializeField] private Constraints constraints;


    private void Update()
    {
        if (!photonView.IsMine)
        {
            var dir = transform.position - PlayerManager.LocalPlayerInstance.transform.position;
            var rotationEuler = Quaternion.LookRotation(dir).eulerAngles;
            

            if(constraints.X) rotationEuler.x = 0;
            if(constraints.Y) rotationEuler.y = 0;
            if(constraints.Z) rotationEuler.z = 0;

            transform.rotation = Quaternion.Euler(rotationEuler);   
        }
    }
}
