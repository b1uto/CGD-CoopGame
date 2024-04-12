using Photon.Pun;
using Photon.Realtime;
using CGD.Networking;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using CGD;
using Photon.Pun.UtilityScripts;

public class RoomMenu : MenuPanel
{
    [SerializeField] private TextMeshProUGUI roomNameLabel;
    [SerializeField] private TextMeshProUGUI roomCodeLabel;
    [SerializeField] private GameObject playerRowPrefab;
    [SerializeField] private Transform rowContainer;
    [SerializeField] private GameObject startGameBtn;

    public PlayerRow[] rows;

    private Player[] players;

    private bool teamMode;

    private void Start()
    {
        NetworkManager.OnPlayersUpdated += UpdatePlayers;
        PhotonTeamsManager.PlayerJoinedTeam += PlayerJoinedTeam;
        PhotonTeamsManager.PlayerLeftTeam += PlayerJoinedTeam;
    }
    private void OnDestroy()
    {
        NetworkManager.OnPlayersUpdated -= UpdatePlayers;
        PhotonTeamsManager.PlayerJoinedTeam -= PlayerJoinedTeam;
        PhotonTeamsManager.PlayerLeftTeam -= PlayerJoinedTeam;
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

            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RoomProperties.TeamGame, out object TeamGame))
            {
                teamMode = (bool)TeamGame;
            }

            UpdatePlayers();
        }
    }

    private void PlayerJoinedTeam(Player player, PhotonTeam team) => UpdatePlayers();

    private void UpdatePlayers() 
    {
        var players = PhotonNetwork.PlayerList;
        int i = 0, blue = 0, red = 0;


        foreach (var row in rows)
        {
            if (i >= players.Length)
                row.gameObject.SetActive(false);
            else
            {
                row.gameObject.SetActive(true);

                var team =  players[i].GetPhotonTeam();

                if (team != null && team.Code == 1)
                    row.DrawRow(players[i], ++blue, 1);
                else if (team != null && team.Code == 2)
                    row.DrawRow(players[i], ++red, 2);
                else
                    row.DrawRow(players[i], i + 1, 0);
            }

            i++;
        }

    }



}

/*
     int itCount = Mathf.Max(players.Length, playerPool.Count);

            for (int i = 0; i < itCount; i++)
            {
                if (i >= playerPool.Count) //instantiate new row
                {
                    var row = Instantiate(playerRowPrefab, rowContainer).GetComponent<PlayerRow>();
                    playerPool.Add(row);
                    row.DrawRow(players[i]);
                }
                else if (i < players.Length) //update row
                {
                    var row = playerPool[i];
                    row.DrawRow(players[i]);
                    row.gameObject.SetActive(true);
                }
                else if (i >= players.Length && i < playerPool.Count) //turn off unused rows
                {
                    playerPool[i].gameObject.SetActive(false);
                }
            }
 */