using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomMenu : Menu
{
    [SerializeField] private NetworkManager networkManager;
    [SerializeField] private TextMeshProUGUI roomNameLabel;
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
        NetworkManager.OnPlayersUpdated += UpdateRoomMenu;
    }

    public override void OnMenuChanged(string newMenuAlias)
    {
        base.OnMenuChanged(newMenuAlias);

        UpdateRoomMenu();
    }

    private void UpdateRoomMenu() 
    {
        if (PhotonNetwork.CurrentRoom != null)
        {
            startGameBtn.SetActive(PhotonNetwork.IsMasterClient);

            string roomName = $"{PhotonNetwork.CurrentRoom.Name} {PhotonNetwork.CurrentRoom.PlayerCount}/{PhotonNetwork.CurrentRoom.MaxPlayers}";
            roomNameLabel.text = roomName;

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
