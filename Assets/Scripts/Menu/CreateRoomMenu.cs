using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRoomMenu : Menu
{
    [SerializeField] private TMPro.TMP_InputField inputField;


    public void Btn_CreateRoom() 
    {
        var networkManager = FindObjectOfType<NetworkManager>();

        if(networkManager && !string.IsNullOrEmpty(inputField.text)) 
        {
            networkManager.CreateRoom(inputField.text);
        }
    }

}
