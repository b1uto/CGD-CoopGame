using Photon.Pun;
using UnityEngine;

namespace CGD.CoopGame
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
    {
        [Header("Movement Settings")]
        [SerializeField] private float groundSpeed = 10f;
        [SerializeField] private float acceleration = 2f;

        [Header("Camera Settings")]
        [SerializeField] private float rotationSpeed;
        [SerializeField] private bool invertPitch;
        [SerializeField] private float maxPitch;
        [SerializeField] private float minPitch;
        [SerializeField] private Camera mainCamera;

        private CharacterController characterController;
        private float pitchValue;
        private Vector3 moveInput;
        private Vector3 velocity;
        private int InvertPitch { get { return invertPitch ? -1 : 1; } }


        private void Start()
        {
            characterController = GetComponent<CharacterController>();
            if (!photonView.IsMine)
            {
                GetComponentInChildren<Camera>().enabled = false;
                GetComponentInChildren<AudioListener>().enabled = false;
            }
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;
            
            HandleCharacterRotation();
            HandleCharacterMovement();
        }
        private void HandleCharacterRotation() 
        {
            transform.Rotate(
                      new Vector3(0f, (Input.GetAxis("Mouse X") * rotationSpeed /** RotationMultiplier*/),
                          0f), Space.Self);

            pitchValue += Input.GetAxis("Mouse Y") * rotationSpeed * InvertPitch /** RotationMultiplier*/;
            pitchValue = Mathf.Clamp(pitchValue, minPitch, maxPitch);
            mainCamera.transform.localEulerAngles = new Vector3(pitchValue, 0, 0);

        }
        private void HandleCharacterMovement()
        {

            moveInput.x = Input.GetAxis("Horizontal");
            moveInput.z = Input.GetAxis("Vertical");
            moveInput = Vector3.ClampMagnitude(moveInput, 1);
            Vector3 worldInput = transform.TransformVector(moveInput);

            //if (/*IsGrounded*/)
            {
                Vector3 targetVelocity = worldInput * groundSpeed;
                velocity = Vector3.Lerp(velocity, targetVelocity, acceleration * Time.deltaTime);
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
