using Photon.Pun;
using UnityEngine;


//TODO dynamic fp camera. head bob, visible body, dynamic fov

namespace CGD.CoopGame
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

        #region Setup
        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            playerInputHandler = GetComponent<PlayerInputHandler>();
        }

        private void Start()
        {
            if (!photonView.IsMine)
            {
                GetComponentInChildren<Camera>().enabled = false;
                GetComponentInChildren<AudioListener>().enabled = false;
                GetComponent<PlayerInputHandler>().enabled = false;
            }
        }
        #endregion


        private void Update()
        {
            if (!photonView.IsMine)
                return;
            
            HandleCharacterRotation();
            HandleCharacterMovement();
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
    }
}
