using CGD.Gameplay;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class HudPanel : MenuPanel
{
    public TextMeshProUGUI gameTimerTMP;

    private float timeLeft;
    private int minutes, seconds;


    private void Update()
    {
        timeLeft = Mathf.Max(0, (float)(GameManager.Instance.GameSettings.RoundEndTime - PhotonNetwork.Time));


        minutes = Mathf.FloorToInt(timeLeft / 60);
        seconds = Mathf.FloorToInt(timeLeft % 60);

        gameTimerTMP.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
