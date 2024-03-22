using CGD.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoinFriendsMenu : MenuPanel
{
    [SerializeField] private TMPro.TMP_InputField inputField;


    public void Btn_JoinWithInviteCode()
    {
        if (!string.IsNullOrEmpty(inputField.text))
        {
            NetworkManager.JoinPrivateRoom(inputField.text);
        }
    }
}
