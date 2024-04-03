using PlayFab.GroupsModels;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CGD.Input
{
    public enum GameContext
    {
        None,
        Game,
        UI
    }
    public class InputManager : Singleton<InputManager>
    {
        #region Delegates
        public delegate void OnChangedContext(GameContext context);
        public static event OnChangedContext OnChangedInputContext;
        #endregion


        /// <summary>
        /// Default Input Actions
        /// </summary>
        private Default_IA inputActionAsset;

        /// <summary>
        /// Current Active Action Map
        /// </summary>
        private GameContext context = GameContext.UI;

        /// <summary>
        /// prevents context being changed.
        /// </summary>
        private bool lockContext = false;


        #region Properties
        public Default_IA InputActionAsset
        {
            get { return inputActionAsset; }
        }
        #endregion

        #region Initialisation
        protected override void CustomAwake() => inputActionAsset = new Default_IA();
        private void OnEnable() => inputActionAsset.Enable();
        private void OnDisable() => inputActionAsset.Disable();

        private void Start()
        {
            GameManagerEvents.OnGameStateChanged += GameStateChanged;
        }
        private void OnDestroy()
        {
            GameManagerEvents.OnGameStateChanged -= GameStateChanged;
        }
        #endregion

        #region Public Functions
        public void SetActiveMap(GameContext context, bool force = false)
        {
            if(lockContext && !force) { return; }

            this.context = context;

            ToggleCursor();

            var contextName = context.ToString();

            foreach (var actionMap in inputActionAsset.asset.actionMaps)
            {
                if (actionMap.name == contextName)
                    actionMap.Enable();
                else
                    actionMap.Disable();
            }

            OnChangedInputContext?.Invoke(context);
        }
        public void DisableAllInput() 
        {
            SetActiveMap(GameContext.None, true);
            lockContext = true;
        }
        #endregion
        #region Private Functions
        private void ToggleCursor() 
        { 
            if(context == GameContext.Game) 
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else 
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
        #endregion


        #region GameManager Callbacks
        private void GameStateChanged(GameState state)
        {
            switch (state)
            {
                case GameState.Loading:
                case GameState.Countdown:
                    DisableAllInput();
                    break;
                case GameState.Start:
                    lockContext = false;
                    SetActiveMap(GameContext.Game);
                    break;
                case GameState.Meeting:
                    SetActiveMap(GameContext.UI);
                    lockContext = true;
                    break;
            }
        }
        #endregion
    }
}