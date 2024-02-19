using Photon.Realtime;
using CGD.Networking;
using System.Collections.Generic;
using UnityEngine;

public class JoinRoomMenu : Menu
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform rowContainer;

    private List<RoomRow> rowPool = new List<RoomRow>();

    private void Start()
    {
       NetworkManager.OnRoomsUpdated += UpdateRoomList;
    }

    private void OnDestroy()
    {
      NetworkManager.OnRoomsUpdated -= UpdateRoomList;
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

    public void JoinRoom(RoomInfo roomInfo) => NetworkManager.JoinRoom(roomInfo);


}
