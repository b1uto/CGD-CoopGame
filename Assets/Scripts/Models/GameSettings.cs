using CGD.Case;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings", order = 1)]
[System.Serializable]
public class GameSettings : ScriptableObject
{
    #region PunEvent Bytes
    public const byte PunLoadScene = 1;
    public const byte PunPlayerLoaded = 2;
    public const byte PunAllPlayersLoaded = 3;
    public const byte PunGameStarted = 4;
    public const byte GameMeetingFinished = 5;
    public const byte PlayerSubmittedClue = 6;
    public const byte PlayerSharedClue = 7;
    public const byte OnNextPlayersTurn = 8;
    public const byte OnPlayerSkippedTurn = 9;
    public const byte PlayerSubmittedSolution = 10;
    public const byte PlayerSolvedCase = 11;
    #endregion

    [SerializeField] private double countdownDuration = 10;
    [SerializeField] private double roundDuration = 30;
    [SerializeField] private double turnDuration = 30;
    [SerializeField] private double turnBufferDuration = 5;
    [SerializeField] private int movesPerTurn = 3;


    #region Runtime
    /// <summary>
    /// synchronised time when all players loaded into scene
    /// </summary>
    private double punStartTime;

    /// <summary>
    /// synced network time + countdown
    /// </summary>
    private double gameStartTime;

    /// <summary>
    /// end of first round time.
    /// </summary>
    private double roundEndTime;

    /// <summary>
    /// end of current players turn.
    /// </summary>
    private double turnEndTime;
    #endregion

    public double GameStartTime { get { return gameStartTime; } }
    public double RoundEndTime { get { return roundEndTime; } }
    public double TurnEndTime { get { return turnEndTime; } }
    public double TurnTime { get { return turnBufferDuration + turnDuration; } }
    public int MovesPerTurn { get {  return movesPerTurn; } }

    public void SetGameTimes(double networkTime) 
    {
        punStartTime = networkTime;
        gameStartTime = networkTime + countdownDuration;
        roundEndTime = networkTime + countdownDuration + roundDuration;
    }

    public void UpdateRoundTime(double networkTime)
    {
        roundEndTime = networkTime + roundDuration;
    }


    public void SetTurnEndTime(double networkTime) 
    {
        turnEndTime = networkTime + turnBufferDuration + turnDuration;
    }



    #region Static RaiseEvent Functions
    private static RaiseEventOptions defaultRaiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };

    public static void RE_PunLoadScene(int sceneIndex) 
    {
        PhotonNetwork.RaiseEvent(PunLoadScene, 
            2, defaultRaiseEventOptions, SendOptions.SendReliable);
    }
    public static void RE_PunPlayerLoaded()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent(PunPlayerLoaded, null, 
            raiseEventOptions, SendOptions.SendReliable);
    }
    public static void RE_PunAllPlayersLoaded(double networkTime)
    {
        PhotonNetwork.RaiseEvent(GameSettings.PunAllPlayersLoaded, 
            networkTime, defaultRaiseEventOptions, SendOptions.SendReliable);
    }
    public static void RE_PunGameStarted(){}
    public static void RE_GameMeetingFinished(double networkTime)
    {
        PhotonNetwork.RaiseEvent(GameMeetingFinished,
               networkTime, defaultRaiseEventOptions, SendOptions.SendReliable);
    }
    public static void RE_PlayerSubmittedClue(string clueId, int actorNumber, bool analysed)
    {
        PhotonNetwork.RaiseEvent(PlayerSubmittedClue, 
            new object[] {clueId, actorNumber, analysed}, defaultRaiseEventOptions, SendOptions.SendReliable);
    }
    public static void RE_PlayerSharedClue(string clueId, int actorNumber, bool analysed)
    {
        PhotonNetwork.RaiseEvent(PlayerSharedClue,
            new object[] { clueId, actorNumber, analysed }, defaultRaiseEventOptions, SendOptions.SendReliable);
    }
    public static void RE_OnNextPlayersTurn(int actorNumber, double networkTime)
    {
        PhotonNetwork.RaiseEvent(OnNextPlayersTurn,
                new object[] { actorNumber, networkTime}, defaultRaiseEventOptions, SendOptions.SendReliable);
    }
    public static void RE_OnPlayerSkippedTurn(int actorNumber)
    {
        PhotonNetwork.RaiseEvent(OnPlayerSkippedTurn,
              actorNumber, defaultRaiseEventOptions, SendOptions.SendReliable);
    }
    public static void RE_PlayerSubmittedSolution(int actorNumber, string[] itemNames)
    {
        var data = new object[] { actorNumber, itemNames };

        PhotonNetwork.RaiseEvent(PlayerSubmittedSolution,
            new object[] { actorNumber, itemNames }, defaultRaiseEventOptions, SendOptions.SendReliable);
    }
    public static void RE_PlayerSolvedCase(int actorNumber, bool caseSolved)
    {
        PhotonNetwork.RaiseEvent(PlayerSolvedCase, 
            new object[]{ actorNumber, caseSolved}, defaultRaiseEventOptions, SendOptions.SendReliable);
    }
    #endregion
}
