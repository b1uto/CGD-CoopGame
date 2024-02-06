using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using UnityEngine;
using CGD;


public class GameManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;

    /// <summary>
    /// TODO store elsewhere in Item Collection. Randomise if player has not chosen avatar.
    /// </summary>
    private static string[] modelNames = new string[5]
    {
        "female01",
        "female02",
        "male01",
        "male02",
        "male03"
    };

    private void Start()
    {
        if(PhotonNetwork.CurrentRoom != null)
            InstantiateNewPlayer();
        else
            PhotonNetwork.JoinRoom(PlayerPrefs.GetString(RoomProperties.RoomKey));
    }

    private void InstantiateNewPlayer()
    {
        if (PlayerManager.LocalPlayerInstance == null)
        {
#if DEBUGGING
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
#endif
            var pos = Random.insideUnitCircle * Random.Range(1, 5);

            PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(pos.x, 0, pos.y)
                , Quaternion.identity, 0);
        }
        else
        {
#if DEBUGGING
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
#endif
        }
    }

    private void Update()
    {
       if(Input.GetKeyDown(KeyCode.P)) { LeaveRoom(); }
    }

    #region Photon Callbacks
    public override void OnLeftRoom()
    {
#if DEBUGGING
        Debug.Log("left room. returning to main menu");
#endif
        SceneManager.LoadScene(0);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
     {
#if DEBUGGING
        DebugCanvas.Instance.AddConsoleLog("Player Joined: " + newPlayer.NickName);
#endif
    }
    public override void OnJoinedRoom()
    {
#if DEBUGGING
        Debug.Log("Late Joiner, Instantiate Player Character");
#endif
        InstantiateNewPlayer(); 
    }
    #endregion
    #region Public Methods
    public void LeaveRoom() => PhotonNetwork.LeaveRoom();

    //TODO change to some form of saved names.
    public static string GetRandomModelName() 
    {
        return modelNames[Random.Range(0, modelNames.Length)];
    }
    #endregion

}
