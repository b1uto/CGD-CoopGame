using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGD.Events;
using Unity.Burst.CompilerServices;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Manager Settings")]
    [SerializeField] private Menu[] menus;
    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private bool openFirstMenu = true;

    private StringEvent OnMenuChanged = new StringEvent();
    public StringEvent OnMenuClosed = new StringEvent();

    private string currentMenu = "";



    private void Start()
    {
        foreach (var menu in menus)
        {
            OnMenuChanged.AddListener(menu.OnMenuChanged);
        }
        
        if(openFirstMenu)
            OpenMenu(0);
        
        mainCanvas.enabled = true;
        Cursor.lockState = CursorLockMode.None;
    }


    public void OpenMenu(int index) 
    {
        if(index < menus.Length) 
        {
            OpenMenu(menus[index].Alias);
        }
    }
    public void OpenMenu(string alias) 
    {
        if(alias != currentMenu) 
        {
            OnMenuChanged.Invoke(alias);
            currentMenu = alias;
        }
    }

    public void CloseMenu() 
    {
        OnMenuClosed?.Invoke(currentMenu);
        currentMenu = "";
        OnMenuChanged?.Invoke("");
    }



}
