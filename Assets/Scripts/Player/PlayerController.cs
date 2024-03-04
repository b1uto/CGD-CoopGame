using Photon.Pun;
using UnityEngine;


//TODO dynamic fp camera. head bob, visible body, dynamic fov

namespace CGD
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Header("Movement Settings")]
        [SerializeField] private float groundSpeed = 10f;
        [SerializeField] private float acceleration = 2f;

        [Header("Camera Settings")]
        [SerializeField] private float maxPitch;
        [SerializeField] private float minPitch;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private float smoothTime = 0.3f;
        [SerializeField] private float smoothVelocity;

        private CharacterController characterController; 
        private PlayerInputHandler playerInputHandler;
        
        private float pitchValue;


        private Vector3 velocity;
        private Vector3 moveVelocity;


        /// <summary>
        /// Physics Layer for interactable objects
        /// </summary>
        private LayerMask layerMask;
        private IInteractable interactable;

        //Equipment
        private IEquippable rightHandTool;


        public Vector2 GroundVelocity 
        {
            get { return new Vector2(velocity.x, velocity.z); }
        }

        #region Setup
        private void Awake()
        {
            layerMask = LayerMask.GetMask("Interactable");
        }

        private void Start()
        {
            if (photonView && !photonView.IsMine)
            {
                GetComponentInChildren<Camera>().enabled = false;
                GetComponentInChildren<AudioListener>().enabled = false;
            }

            characterController = GetComponent<CharacterController>();
            playerInputHandler = GetComponent<PlayerInputHandler>();
        }
        #endregion


        private void Update()
        {
            if (!photonView.IsMine)
                HandleClientSync();
            else
            {
                HandleCharacterRotation();
                HandleCharacterMovement();
            }
        }

        private void FixedUpdate()
        {
            LineOfSightRay();
        }

        #region Character Movement
        private void HandleClientSync() 
        {
            var oldPitchValue = mainCamera.transform.localEulerAngles.x;
            oldPitchValue = (oldPitchValue > 180) ? oldPitchValue - 360 : oldPitchValue;

            var targetPitchValue = Mathf.SmoothDamp(oldPitchValue, pitchValue, ref smoothVelocity, smoothTime);
            mainCamera.transform.localEulerAngles = new Vector3(targetPitchValue, 0, 0);
        }
        private void HandleCharacterRotation() 
        {
            var lookInput = playerInputHandler.LookInput;

            transform.Rotate(
                      new Vector3(0f, lookInput.x /** RotationMultiplier*/,
                          0f), Space.Self);

            pitchValue += lookInput.y; /** RotationMultiplier*/;
            pitchValue = Mathf.Clamp(pitchValue, minPitch, maxPitch);
            mainCamera.transform.localEulerAngles = new Vector3(pitchValue, 0, 0);

        }
        private void HandleCharacterMovement()
        {
            Vector3 worldInput = transform.TransformVector(playerInputHandler.MoveInput);

            //if (/*IsGrounded*/)
            {
                Vector3 targetVelocity = worldInput * groundSpeed;
                velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref moveVelocity, acceleration);
            }

            velocity.y += Physics.gravity.y * Time.deltaTime;
            characterController.Move(velocity * Time.deltaTime);
        }
        #endregion

        #region Interaction
        private void LineOfSightRay() 
        {
            var start = mainCamera.transform.position;
            var direction = mainCamera.transform.forward;

            if (Physics.Raycast(start, direction, out RaycastHit hitData, 25, layerMask)) 
            {
                if(hitData.transform.TryGetComponent(out IInteractable interactableObj)) 
                {
                    OnFoundInteractable(interactableObj);
                }
                if (hitData.transform.TryGetComponent(out IEquippable equippableObj))
                {
                    OnFoundInteractable(interactableObj);
                }
            }
            else 
            {
                LostInteractable();
            }
        }

        /// <summary>
        /// Currently looking at an interactable object
        /// </summary>
        /// <param name="newInteractable"></param>
        private void OnFoundInteractable(IInteractable newInteractable) 
        {
            if(newInteractable != interactable) 
            {
                if(interactable != null) 
                    interactable.OnExitFocus();

                newInteractable.OnFocus();
                interactable = newInteractable; 
            }
        }

        private void LostInteractable() 
        {
            if(interactable != null)
            {
                interactable.OnExitFocus();
                interactable = null;
            }
        }

        public void Interact() 
        {
            if (interactable != null) 
                interactable.Interact(photonView.ViewID);
        }
        public void Equip()
        {
            if (interactable != null && interactable is IEquippable equippableObj)
            {
                Drop();
                //var slot = GetComponent<PlayerAnimController>().GetEquipSlot(equippableObj.ItemEquipSlot);
                equippableObj.Equip(photonView.ViewID);
                rightHandTool = equippableObj;
            }
        }

        public void Drop() 
        {
            //TODO extend to multiple slots
            if (rightHandTool != null)
            {
                rightHandTool.Unequip();
            }
        }


        public void Fire()
        {
            if (rightHandTool != null)
                rightHandTool.Interact(photonView.ViewID);
        }


        #endregion



        #region Photon Synchronisation
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(pitchValue);
            }
            else
            {
                float networkPitch = (float)stream.ReceiveNext();
                pitchValue = Mathf.Clamp(networkPitch, minPitch, maxPitch);
            }
        }
        #endregion
    }
}


/*
  public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                pitchValueDelta = pitchValue - pitchValueLastSync;
                pitchValueLastSync = pitchValue;

                stream.SendNext(pitchValue);
                stream.SendNext(pitchValueDelta);
            }
            else
            {
                float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

                float networkPitch = (float)stream.ReceiveNext();
                float networkPitchDelta = (float)stream.ReceiveNext();

                pitchValue = Mathf.Clamp(networkPitch, minPitch, maxPitch);
            }
        }*/