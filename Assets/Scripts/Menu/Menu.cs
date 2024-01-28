using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public string Alias { get { return alias; } }   
    [SerializeField] private string alias;


    public virtual void OnMenuChanged(string newMenuAlias) 
    {
        //TODO update to include animations/transitions
        gameObject.SetActive(alias == newMenuAlias);
    }
}
