using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace CGD.Input
{ 

    public class PlayerInputHandler : MonoBehaviour
    {
        public bool Debugging = false;

        /// <summary>
        /// Reference to pawn controller
        /// </summary>
        [SerializeField] private PlayerController playerController;

        /// <summary>
        /// ScriptableObject. input settings
        /// </summary>
        [SerializeField] private InputSettings settings;

        /// <summary>
        /// Default Input Actions
        /// </summary>
        //private Default_IA playerInput;

        /// <summary>
        /// current movement input
        /// </summary>
        private Vector2 moveInput;

        /// <summary>
        /// Used for smoothing out movement input.
        /// </summary>
        private Vector2 moveVelocity;

        /// <summary>
        /// current look input
        /// </summary>
        private Vector2 lookInput;

        /// <summary>
        /// Used for converting vector2 input to Vector3
        /// </summary>
        private Vector3 movement3;


        #region Properties
        /// <summary>
        /// Converts Input to Vector3
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
            settings = ItemCollection.Instance.InputSettings;
            playerController = GetComponent<PlayerController>();
        }
        private void Start()
        {
            InputManager.OnChangedInputContext += OnChangedInputContext;
            InitialiseGameActionMap();
        }

        private void OnDestroy()
        {
            InputManager.OnChangedInputContext -= OnChangedInputContext;
            ClearGameActionMap();
        }
        private void InitialiseGameActionMap() 
        {
            var Game = InputManager.Instance.InputActionAsset.Game;

            Game.Move.performed += OnMoveInput;
            Game.Look.performed += OnLookInput;
            Game.Interact.performed += OnInteractInput;
            Game.Equip.performed += OnEquipInput;
            Game.Drop.performed += OnDropInput;
            Game.Fire.performed += OnFireInput;
        }
        private void ClearGameActionMap()
        {
            var Game = InputManager.Instance.InputActionAsset.Game;

            Game.Move.performed -= OnMoveInput;
            Game.Look.performed -= OnLookInput;
            Game.Interact.performed -= OnInteractInput;
            Game.Equip.performed -= OnEquipInput;
            Game.Drop.performed -= OnDropInput;
            Game.Fire.performed -= OnFireInput;
        }
        #endregion

        #region Input Callbacks
        private void OnMoveInput(InputAction.CallbackContext context)
        {
            moveInput = Vector3.ClampMagnitude(context.ReadValue<Vector2>(), 1);
        }
        private void OnLookInput(InputAction.CallbackContext context)
        {
            lookInput = context.ReadValue<Vector2>() * Time.deltaTime;
            lookInput.x *= settings.SensitivityX;
            lookInput.y *= settings.SensitivityY * settings.InvertY;
        }

        private void OnInteractInput(InputAction.CallbackContext context)
        {
            if(context.interaction is PressInteraction) 
                playerController.Interact();
        }
        private void OnEquipInput(InputAction.CallbackContext context)
        {
            playerController.Equip();
        }
        private void OnDropInput(InputAction.CallbackContext context)
        {
            playerController.Drop();
        }
        private void OnFireInput(InputAction.CallbackContext context)
        {
            playerController.Fire();
        }
        #endregion

        #region Action Map
        private void OnChangedInputContext(GameContext context) 
        {
            if(context != GameContext.Game)
                moveInput = moveVelocity = lookInput = movement3 = Vector3.zero;
        }
        //public void DisablePlayerInput()
        //{
        //    moveInput = moveVelocity = lookInput = movement3 = Vector3.zero;
        //    InputManager.Instance.SetActiveMap(GameContext.UI);
        //}
        //public void EnablePlayerInput()
        //{
        //    InputManager.Instance.SetActiveMap(GameContext.Game);
        //}
        #endregion
    }
}