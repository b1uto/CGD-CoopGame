using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace CGD
{
    public class PlayerInputHandler : MonoBehaviour
    {
        /// <summary>
        /// ScriptableObject. input settings
        /// </summary>
        [SerializeField] private InputSettings settings;


        /// <summary>
        /// Default Input Actions
        /// </summary>
        private Default_IA playerInput;


        private Vector2 moveInput;

        /// <summary>
        /// Used for smoothing out movement input.
        /// </summary>
        private Vector2 moveVelocity;

        private Vector2 lookInput;
        /// <summary>
        /// Used for converting vector2 input to Vector3
        /// </summary>
        private Vector3 movement3;


        #region Properties
        /// <summary>
        /// Get Converted Input Value
        /// </summary>
        public Vector3 MoveInput
        {
            get
            {
                movement3.x = moveInput.x;
                movement3.y = 0;
                movement3.z = moveInput.y;
                return movement3;
            }
        }

        /// <summary>
        /// Get Look Value
        /// </summary>
        public Vector3 LookInput
        {
            get
            {
                return lookInput;
            }
        }
        #endregion

        #region Setup
        private void Awake()
        {
            var filePath = System.IO.Path.Combine("Data", "Settings");
            settings = Resources.Load<InputSettings>(System.IO.Path.Combine(filePath, "InputSettings"));
        }


        private void Start()
        {
            playerInput = new Default_IA();
            playerInput.Default.Move.performed += OnMoveInput;
            playerInput.Default.Look.performed += OnLookInput;
            playerInput.Default.Interact.performed += OnInteractInput;
            playerInput.Default.Equip.performed += OnEquipInput;
            playerInput.Default.Drop.performed += OnDropInput;
            playerInput.Default.Fire.performed += OnFireInput;
            playerInput.Enable();
        }
        private void OnEnable()
        {
            if (playerInput != null) playerInput.Enable();
        }
        private void OnDisable()
        {
            if (playerInput != null) playerInput.Disable();
        }
        #endregion

        private void Update()
        {
            DebugCanvas.Instance.UpdateInputText(
                $"Move: {moveInput}" +
                $"\nLook: {lookInput}");
        }

        #region Input
        private void OnMoveInput(InputAction.CallbackContext context)
        {
            moveInput = Vector3.ClampMagnitude(context.ReadValue<Vector2>(), 1);
        }
        private void OnLookInput(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>() * Time.deltaTime;
            lookInput.x *= settings.sensitivityX;
            lookInput.y *= settings.sensitivityY * settings.InvertY;
        }

        private void OnInteractInput(InputAction.CallbackContext context)
        {
            if(context.interaction is TapInteraction) 
                GetComponent<PlayerController>().Interact();
        }
        private void OnEquipInput(InputAction.CallbackContext context)
        {
            GetComponent<PlayerController>().Equip();
        }
        private void OnDropInput(InputAction.CallbackContext context)
        {
            GetComponent<PlayerController>().Drop();
        }
        private void OnFireInput(InputAction.CallbackContext context)
        {
            GetComponent<PlayerController>().Fire();
        }
        #endregion
    }
}