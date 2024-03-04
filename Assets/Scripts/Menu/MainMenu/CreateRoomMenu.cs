using CGD.Networking;
using UnityEngine;

public class CreateRoomMenu : MenuPanel
{
    [SerializeField] private TMPro.TMP_InputField inputField;


    public void Btn_CreateRoom() 
    {

        if(!string.IsNullOrEmpty(inputField.text)) 
        {
            NetworkManager.CreateRoom(inputField.text);
        }
    }

}
