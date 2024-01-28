using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public class JoinRoomMenu : Menu
{
    [SerializeField]private NetworkManager networkManager;
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform rowContainer;

    private List<RoomRow> rowPool = new List<RoomRow>();

    private void Start()
    {
        networkManager.RoomListUpdated += UpdateRoomList;
    }

    private void OnDestroy()
    {
        networkManager.RoomListUpdated -= UpdateRoomList;
    }
    private void UpdateRoomList(List<RoomInfo> roomList)  
    {
        int itCount = (roomList.Count > rowPool.Count) ? roomList.Count : rowPool.Count;

        for(int i = 0; i < itCount; i++) 
        {
            if(i >= rowPool.Count) //instantiate new row
            {
                var row = Instantiate(rowPrefab, rowContainer).GetComponent<RoomRow>();
                rowPool.Add(row);
                row.Draw(roomList[i], this);
            }
            else if(i < roomList.Count) //update row
            {
                var row = rowPool[i];
                row.Draw(roomList[i], this);
                row.gameObject.SetActive(true);
            }
            else if (i >= roomList.Count && i < rowPool.Count) //turn off unused rows
            {
                rowPool[i].gameObject.SetActive(true);
            }
        }
    }

    public void JoinRoom(RoomInfo roomInfo) => networkManager.JoinRoom(roomInfo);


}
