using CGD.Networking;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomRow : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI playerCountLabel;
    
    private RoomInfo roomInfo;
    private bool onClickAdded = false;

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    public void Draw(RoomInfo roomInfo, JoinRoomMenu joinRoomMenu)
    {
        this.roomInfo = roomInfo;

        if (roomInfo.CustomProperties.TryGetValue(RoomProperties.RoomName, out object roomName))
        {
            nameLabel.text = (string)roomName;
        }
        else
        {
            nameLabel.text = "*";
        }

        playerCountLabel.text = $"{roomInfo.PlayerCount}/{roomInfo.MaxPlayers}";

        if (!onClickAdded)
        {
            button.onClick.AddListener(() => joinRoomMenu.JoinRoom(this.roomInfo));
            onClickAdded = true;
        }
    }

}
