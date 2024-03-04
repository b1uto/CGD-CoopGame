using UnityEngine;
using CGD.Events;

[System.Serializable]
public class Menu 
{
    public string alias;
    public MenuPanel panel;    
}

public class MenuManager : Singleton<MenuManager>
{
    [Header("Manager Settings")]
    [SerializeField] protected Menu[] menus;
    [SerializeField] protected Canvas mainCanvas;
    [SerializeField] protected bool openFirstMenu = true;

    protected StringEvent OnMenuPanelChanged = new StringEvent();
    protected string currentMenu = "";


    public void Start()
    {
        foreach (var menu in menus)
        {
            OnMenuPanelChanged.AddListener((x) =>
            {
                menu.panel.TogglePanel(x == menu.alias);
            });

            menu.panel.gameObject.SetActive(false);
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
            OpenMenu(menus[index].alias);
        }
    }
    public void OpenMenu(string alias) 
    {
        if(alias != currentMenu)
        {
            currentMenu = alias;
            OnMenuPanelChanged.Invoke(alias);
        }
    }


}
