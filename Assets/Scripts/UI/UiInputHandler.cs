using CGD.Input;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class UiInputHandler : MonoBehaviour
{
    /// <summary>
    /// may change to class ref
    /// </summary>
    [SerializeField] private GameObject pauseMenu;

    /// <summary>
    /// may change to class ref
    /// </summary>
    [SerializeField] private Canvas hudCanvas;


    private void Start()
    {
        InitialiseUIMap();
    }

    private void OnDestroy()
    {
        ClearUIMap();
    }

    private void InitialiseUIMap() 
    {
        var UI = InputManager.Instance.InputActionAsset.UI;
        var Game = InputManager.Instance.InputActionAsset.Game;

        UI.Menu.performed += OnMenuPressed;
        Game.Menu.performed += OnMenuPressed;
    }
    private void ClearUIMap()
    {
        var UI = InputManager.Instance.InputActionAsset.UI;
        var Game = InputManager.Instance.InputActionAsset.Game;

        UI.Menu.performed -= OnMenuPressed;
        Game.Menu.performed -= OnMenuPressed;
    }

    private void OnMenuPressed(InputAction.CallbackContext context) => TogglePauseMenu();
    private void TogglePauseMenu() => SetPauseMenuState(!pauseMenu.activeSelf);

    public void SetPauseMenuState(bool show)
    {
        pauseMenu.SetActive(show);
        hudCanvas.enabled = !show;
        InputManager.Instance.SetActiveMap(show ? GameContext.UI : GameContext.Game);
    }
}
