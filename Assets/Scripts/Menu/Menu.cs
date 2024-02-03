using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string Alias { get { return alias; } }   
    [SerializeField] private string alias;

    /// <summary>
    /// Call on all child classes to initialise buttons.
    /// </summary>
    protected void AddButtonOnClickListeners() { }

    public virtual void OnMenuChanged(string newMenuAlias) 
    {
        //TODO update to include animations/transitions
        gameObject.SetActive(alias == newMenuAlias);
    }



}
