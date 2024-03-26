using CGD.Case;
using UnityEngine.Events;

namespace CGD
{
    public class GameMenuManager : MenuManager
    {
        /// <summary>
        /// Casts Base static instance to this class
        /// </summary>
        public new static GameMenuManager Instance
        {
            get
            {
                return (GameMenuManager)_instance;
            }
        }

        private void OnEnable()
        {
            GameManager.OnGameStateChanged += GameStateChanged;
        }
        private void OnDestroy()
        {
            GameManager.OnGameStateChanged -= GameStateChanged;
        }

        private void GameStateChanged(GameState state) 
        {
            switch (state) 
            {
                case GameState.Countdown:
                    OpenMenu("countdown");
                    break;
                case GameState.Start:
                    OpenMenu("hud");
                    break;
                case GameState.Meeting:
                    OpenMenu("board");
                    break;
            }
        }

        public void OpenCluePanel(Clue clue) 
        {
            var cluePanel = GetMenu("clue");

            if (cluePanel != null)
            {
                ((CluePanel)cluePanel).SetPanel(clue.icon, clue.analysedDescription);
                OpenMenu("clue");
            }

        }

        public void CloseMenu() => OpenMenu("");
    }
}
