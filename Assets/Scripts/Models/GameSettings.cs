using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Settings/GameSettings", order = 1)]
[System.Serializable]
public class GameSettings : ScriptableObject
{
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
}
