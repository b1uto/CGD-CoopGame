using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings", order = 1)]
[System.Serializable]
public class GameSettings : ScriptableObject
{
    public const byte PunLoadScene = 1;
    public const byte PunPlayerLoaded = 2;
    public const byte PunAllPlayersLoaded = 3;
    public const byte PunGameStarted = 4;
    public const byte GameMeetingFinished = 5;


    [SerializeField] private double countdownDuration = 10;
    [SerializeField] private double roundDuration = 30;


    /* runtime values */
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
    /// turn time-limit for evidence board round.
    /// </summary>
    private double boardTurnDuration = 30;

    public double GameStartTime { get { return gameStartTime; } }
    public double RoundEndTime { get { return roundEndTime; } }
    public double TurnTime { get { return boardTurnDuration; } }


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



}
