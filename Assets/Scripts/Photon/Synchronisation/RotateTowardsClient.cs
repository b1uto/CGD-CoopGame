using Photon.Pun;
using CGD;
using UnityEngine;

public class RotateTowardsClient : MonoBehaviourPunCallbacks
{
    [System.Serializable]
    public struct Constraints 
    {
        public bool X;
        public bool Y;
        public bool Z;
    }

    [SerializeField] private Constraints constraints;
    [SerializeField] private Vector3 worldOffset;


    private void Update()
    {
        if (worldOffset != Vector3.zero) 
        {
            transform.position = transform.parent.position + worldOffset;
        }


        if (/*photonView != null && !photonView.IsMine &&*/ PlayerManager.LocalPlayerInstance != null)
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
