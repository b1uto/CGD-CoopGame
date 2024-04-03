using CGD.Networking;
using UnityEngine;

public class CreateRoomMenu : MenuPanel
{
    [SerializeField] private TMPro.TMP_InputField inputFieldTMP;
    [SerializeField] private TMPro.TextMeshProUGUI maxPlayersTMP;
    [SerializeField] private TMPro.TextMeshProUGUI gameModeTMP;

    private int MaxPlayers { get { return maxPlayers; } set  { maxPlayers = Mathf.Clamp(value, 4, 10); } }
    private int maxPlayers = 6;
    private bool inviteOnly = false;
    private bool teamsMode = false;


    public void Btn_CreateRoom() 
    {
        if(!string.IsNullOrEmpty(inputFieldTMP.text)) 
        {
            if (teamsMode && MaxPlayers % 2 != 0)
                MaxPlayers++;

            NetworkManager.CreateRoom(inputFieldTMP.text, MaxPlayers, teamsMode, inviteOnly);
        }
    }

    public void Btn_ChangeMaxPlayers(bool increase) 
    {
        MaxPlayers += increase ? 1 : -1;
        maxPlayersTMP.text = MaxPlayers.ToString();
    }

    public void Btn_ChangeMode() 
    {
        teamsMode = !teamsMode;
        gameModeTMP.text = teamsMode ? "teams" : "standard";
    }

    public void Toggle_InviteOnly(bool isOn) => inviteOnly = isOn;

}
