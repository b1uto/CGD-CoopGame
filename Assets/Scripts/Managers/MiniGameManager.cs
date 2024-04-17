using CGD.Case;
using CGD.Input;
using UnityEngine.SceneManagement;

public class MiniGameManager : Singleton<MiniGameManager>
{
    public delegate void MiniGameDelegate(string clueId, bool status);
    public static MiniGameDelegate OnMiniGameFinished;

    private string activeMiniGame = "";

    private string clueId;
    public void LoadMiniGame(Clue clue) 
    {
        clueId = clue.id;
        switch(clue.analyseTool) 
        {
            case AnalyseTool.Swab:
                LoadMiniGame("MG_VolumePuzzle");
                break;
        }
    }

    public void LoadMiniGame(string name)
    {
        if (SceneManager.GetSceneByName(name) != null)
        {
            activeMiniGame = name;
            InputManager.Instance.SetActiveMap(GameContext.UI);
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        }
    }

    public void UnloadMiniGame(bool status) 
    {
        if(!string.IsNullOrEmpty(activeMiniGame)) 
        {
            SceneManager.UnloadSceneAsync(activeMiniGame);
            activeMiniGame = "";
            InputManager.Instance.SetActiveMap(GameContext.Game);
            OnMiniGameFinished?.Invoke(clueId, status); 

        }
    }
}
