using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CGD.Events;

public class MenuManager : Singleton<MenuManager>
{
    [Header("Manager Settings")]
    [SerializeField] private Menu[] menus;
    [SerializeField] private Canvas mainCanvas;

    private StringEvent OnMenuChanged = new StringEvent();
    private string currentMenu = "";

    protected void Awake()
    {
        base.Awake();

        foreach (var menu in menus) 
        {
            OnMenuChanged.AddListener(menu.OnMenuChanged);
        }
    }

    private void Start()
    {
        OpenMenu(0);
        mainCanvas.enabled = true;
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



}
