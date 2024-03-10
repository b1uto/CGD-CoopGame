using Photon.Pun;
using CGD.Networking;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CGD;

public class RoomMenu : MenuPanel
{
    [SerializeField] private TextMeshProUGUI roomNameLabel;
    [SerializeField] private TextMeshProUGUI roomCodeLabel;
    [SerializeField] private GameObject playerRowPrefab;
    [SerializeField] private Transform rowContainer;
    [SerializeField] private GameObject startGameBtn;

    private List<GameObject> playerPool = new List<GameObject>();

    private void Start()
    {
        NetworkManager.OnPlayersUpdated += UpdateRoomMenu;
    }

    private void OnDestroy()
    {
        NetworkManager.OnPlayersUpdated -= UpdateRoomMenu;
    }

    private void OnEnable()
    {
        UpdateRoomMenu();
    }

    private void UpdateRoomMenu() 
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            startGameBtn.SetActive(PhotonNetwork.IsMasterClient);

            roomCodeLabel.text = $"Room Code [{PhotonNetwork.CurrentRoom.Name}]";
            
           if( PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperties.RoomName, out object RoomName))
            {
                string roomName = $"{(string)RoomName} {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
                roomNameLabel.text = roomName;
            }


            var players = PhotonNetwork.PlayerList;

            int itCount = (players.Length > playerPool.Count) ? players.Length : playerPool.Count;

            for (int i = 0; i < itCount; i++)
            {
                string name = players[i].NickName;
                
                if (players[i].IsMasterClient)
                    name += " [HOST]";

                if (i >= playerPool.Count) //instantiate new row
                {
                    var row = Instantiate(playerRowPrefab, rowContainer);
                    playerPool.Add(row);
                    row.GetComponentInChildren<TextMeshProUGUI>().text = name;
                }
                else if (i < players.Length) //update row
                {
                    var row = playerPool[i];
                    row.GetComponentInChildren<TextMeshProUGUI>().text = name;
                    row.gameObject.SetActive(true);
                }
                else if (i >= players.Length && i < playerPool.Count) //turn off unused rows
                {
                    playerPool[i].gameObject.SetActive(true);
                }
            }
        }
    }
}
