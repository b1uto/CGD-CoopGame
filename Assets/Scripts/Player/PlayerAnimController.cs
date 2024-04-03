using DG.Tweening;
using Photon.Pun;
using CGD.Input;
using UnityEngine;

namespace CGD
{
    public class PlayerAnimController : MonoBehaviourPun, IPunObservable
    {
        [SerializeField] private Animator animator;
        [SerializeField] private RigController rigController;

        //TODO change this to be chosen by the host
        // [SerializeField] private GameObject modelPrefab;
        [SerializeField] private float directionDamp;

        [Header("Rig Transforms")]
        [SerializeField] private Transform leftHandSlot;
        [SerializeField] private Transform mainCamera;

        private PlayerInputHandler inputHandler;

        private const string moveX = "MoveX";
        private const string moveY = "MoveY";

        private float xVal, yVal;
        private bool itemEquipped = false;

        private void Start()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
        }

        void Update()
        {
            if (inputHandler != null && (photonView == null || photonView.IsMine))
            {
                //TODO use velocity over input.
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

        public Transform EquipItem(EquipSlot equipSlot)
        {
            DOVirtual.Float(0, 1, 1.0f, val => UpdateArmLayer(val));
            itemEquipped = true;

            switch (equipSlot)
            {
                default:
                    return rigController.ToolParent;
            }
        }

        public void UnEquipItem() 
        {
            DOVirtual.Float(1, 0, 1.0f, val => UpdateArmLayer(val));
            itemEquipped = false;
        }

        public void InitialiseAnimController(PhotonView owner, string modelPath) 
        {
            var model = Resources.Load<GameObject>(modelPath);
            var modelTransform = Instantiate(model, owner.transform).transform;
            modelTransform.localPosition = Vector3.zero;
            modelTransform.localRotation = Quaternion.identity;

            animator = modelTransform.GetComponent<Animator>();
            rigController = modelTransform.GetComponentInChildren<RigController>();
            
            if(rigController != null ) 
                rigController.SetConstraintTargets(leftHandSlot, mainCamera);

        }

        private void UpdateArmLayer(float value) 
        {
            animator.SetLayerWeight(1, value);
            rigController.UpdateRigWeight(value);
        }

    }
}