using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;

    private void Start()
    {
        if (PlayerManager.LocalPlayerInstance == null)
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
            
            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
        }
        else
        {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }

    #region Photon Callbacks
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //if(newPlayer.IsMasterClient) 
        //{
        //    Debug.Log("OnPlayerEnteredRoom IsMasterClient: " + newPlayer.NickName);
        //    LoadArena();
        //}
        //else 
        //{
        //    Debug.Log("OnPlayerEnteredRoom " + newPlayer.NickName);
        //    PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 4, 0), Quaternion.identity);


        //}
    }
    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        //Debug.Log("OnPlayerLeftRoom " + newPlayer.NickName);
        //if (newPlayer.IsMasterClient)
        //{
        //    Debug.Log("OnPlayerLeftRoom IsMasterClient: " + newPlayer.NickName);
        //    LoadArena();
        //}
    }
    #endregion

    #region Private Methods
    private void LoadArena() 
    {
        //if (!PhotonNetwork.IsMasterClient)
        //{
        //    Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        //    return;
        //}
        //Debug.Log("PhotonNetwork : Loading Level :" + PhotonNetwork.CurrentRoom.PlayerCount);

        ////TODO adjust to naming convention for our levels. 
        ////PhotonNetwork.LoadLevel(1);

    }
    #endregion
    #region Public Methods
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();
    #endregion

}
