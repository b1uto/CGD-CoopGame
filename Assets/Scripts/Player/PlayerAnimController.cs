using Photon.Pun;
using UnityEngine;

namespace CGD
{
    public class PlayerAnimController : MonoBehaviour
    {
        public Animator animator;

        //TODO change this to be chosen by the host
        [SerializeField] private GameObject modelPrefab;

        private PlayerInputHandler inputHandler;

        [SerializeField] private float directionDamp;

        [Header("Equipment Slots")]
        public Transform rightHandSlot;



        private const string moveX = "MoveX";
        private const string moveY = "MoveY";

        private void Awake()
        {
            inputHandler = GetComponent<PlayerInputHandler>();
        }
        // Start is called before the first frame update
        void Start()
        {
            //animator = Instantiate(modelPrefab, transform).GetComponent<Animator>();
            //var photonView = GetComponentInParent<PhotonView>();

            //if (photonView && photonView.IsMine)
            //{
            //    photonView.ObservedComponents.Add(animator);
            //}
        }

        // Update is called once per frame
        void Update()
        {
            if (animator == null || inputHandler == null)
                return;

            animator.SetFloat(moveX, inputHandler.MoveInput.x, directionDamp, Time.deltaTime);
            animator.SetFloat(moveY, inputHandler.MoveInput.z, directionDamp, Time.deltaTime);
        }

        public Transform GetEquipSlot(EquipSlot slot) 
        {
            switch (slot) 
            {
                case EquipSlot.RightHand: 
                    return rightHandSlot;
            }

            return null;
        }


        //    public void OnPhotonInstantiate(PhotonMessageInfo info)
        //    {
        //#if DEBUGGING
        //            DebugCanvas.Instance.AddConsoleLog("Calling InitialiseModel RPC");
        //#endif

        //        InitialiseModel(info.Sender.)
        //    }

        //    [PunRPC]
        //    private void InitialiseModel(string goName)
        //    {
        //        PhotonView.Find
        //        var model = GameObject.Find(goName);
        //        if (model)
        //        {
        //            model.transform.SetParent(transform);
        //            model.transform.localPosition = Vector3.zero;
        //            GetComponent<PlayerAnimController>().animator = model.GetComponent<Animator>();
        //        }
        //    }
    }
}