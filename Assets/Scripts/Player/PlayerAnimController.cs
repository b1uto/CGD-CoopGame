using Photon.Pun;
using UnityEngine;

namespace CGD
{
    public class PlayerAnimController : MonoBehaviourPun, IPunObservable
    {
        public Animator animator;

        //TODO change this to be chosen by the host
        [SerializeField] private GameObject modelPrefab;
        [SerializeField] private float directionDamp;

        [Header("Equipment Slots")]
        [SerializeField] private Transform rightHandSlot;
        [SerializeField] private Transform leftHandSlot;

        private PlayerInputHandler inputHandler;

        private const string moveX = "MoveX";
        private const string moveY = "MoveY";

        private float xVal, yVal;

        private void Start()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        void Update()
        {
            if (inputHandler != null && photonView.IsMine)
            {
                xVal = inputHandler.MoveInput.x;
                yVal = inputHandler.MoveInput.z;
            }

            if (animator)
            {
                animator.SetFloat(moveX, xVal, directionDamp, Time.deltaTime);
                animator.SetFloat(moveY, yVal, directionDamp, Time.deltaTime);
            }
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(xVal);
                stream.SendNext(yVal);
            }
            else
            {
                xVal = (float)stream.ReceiveNext();
                yVal = (float)stream.ReceiveNext();
            }
        }

        public Transform GetEquipSlot(EquipSlot equipSlot)
        {
            switch (equipSlot)
            {
                case EquipSlot.RightHand:
                    return rightHandSlot;
                default:
                    return leftHandSlot;
            }
        }


    }
}