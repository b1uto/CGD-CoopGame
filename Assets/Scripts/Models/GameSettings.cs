using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public const byte OnNextPlayersTurn = 7;
    public const byte OnPlayerSkippedTurn = 8;
    #endregion

    [SerializeField] private double countdownDuration = 10;
    [SerializeField] private double roundDuration = 30;
    [SerializeField] private double turnDuration = 30;
    [SerializeField] private double turnBufferDuration = 5;
    [SerializeField] private int minNumOfRounds = 2;


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

    public int MinNumOfRounds { get {  return minNumOfRounds; } }

   // public double TurnTime


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




}
