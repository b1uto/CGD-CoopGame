using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MenuPanel : MonoBehaviour 
{
    public virtual void TogglePanel(bool showPanel) 
    {
        //TODO update to include animations/transitions
        gameObject.SetActive(showPanel);
    }
}
