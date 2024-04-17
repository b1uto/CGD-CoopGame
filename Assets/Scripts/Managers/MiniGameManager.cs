using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGameManager : Singleton<MiniGameManager>
{
    private string activeMiniGame = "";


    public void LoadMiniGame(string name)
    {
        if (SceneManager.GetSceneByName(name) != null)
        {
            activeMiniGame = name;
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        }
    }

    public void UnloadMiniGame() 
    {
        if(!string.IsNullOrEmpty(activeMiniGame)) 
        {
            SceneManager.UnloadSceneAsync(activeMiniGame);
            activeMiniGame = "";
        }
    }
}
