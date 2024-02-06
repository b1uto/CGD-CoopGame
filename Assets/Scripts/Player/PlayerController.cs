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

        #region Setup
        private void Awake()
        {
            layerMask = LayerMask.GetMask("Interactable");
            characterController = GetComponent<CharacterController>();
            playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Start()
        {
            if (photonView && !photonView.IsMine)
            {
                GetComponentInChildren<Camera>().enabled = false;
                GetComponentInChildren<AudioListener>().enabled = false;
                GetComponent<PlayerInputHandler>().enabled = false;
            }

            Cursor.lockState = CursorLockMode.Locked;
        }
        #endregion


        private void Update()
        {
            if (photonView && !photonView.IsMine)
                return;
            
            HandleCharacterRotation();
            HandleCharacterMovement();
        }

        private void FixedUpdate()
        {
            LineOfSightRay();
        }

        #region Character Movement
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
            //moveInput.x = Input.GetAxis("Horizontal");
            //moveInput.z = Input.GetAxis("Vertical");


            //moveInput = Vector3.ClampMagnitude(moveInput, 1);
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
                interactable.Interact();
        }
        public void Equip()
        {
            if (interactable != null && interactable is IEquippable equippableObj)
            {
                Drop();
                var slot = GetComponent<PlayerAnimController>().GetEquipSlot(equippableObj.ItemEquipSlot);
                equippableObj.Equip(slot);
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
                rightHandTool.Interact();
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
                pitchValue = (float)stream.ReceiveNext();
                mainCamera.transform.localEulerAngles = new Vector3(pitchValue, 0, 0);
            }
        }

        #endregion
    }
}   
