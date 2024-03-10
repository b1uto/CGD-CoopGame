using CGD.Networking;
using UnityEngine;

public class CreateRoomMenu : MenuPanel
{
    [SerializeField] private TMPro.TMP_InputField inputFieldTMP;
    [SerializeField] private TMPro.TextMeshProUGUI maxPlayersTMP;

    private int MaxPlayers { get { return maxPlayers; } set  { maxPlayers = Mathf.Clamp(value, 4, 10); } }
    private int maxPlayers = 6;
    private bool inviteOnly = false;

    public void Btn_CreateRoom() 
    {
        if(!string.IsNullOrEmpty(inputFieldTMP.text)) 
        {
            NetworkManager.CreateRoom(inputFieldTMP.text, maxPlayers, inviteOnly);
        }
    }

    public void Btn_ChangeMaxPlayers(bool increase) 
    {
        MaxPlayers += increase ? 1 : -1;
        maxPlayersTMP.text = MaxPlayers.ToString();
    }

    public void Toggle_InviteOnly(bool isOn) => inviteOnly = isOn;

}
