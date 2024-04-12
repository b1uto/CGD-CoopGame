using CGD.Gameplay;
using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownPanel : MenuPanel
{
    [SerializeField] private TextMeshProUGUI countdownTMP;


    private void OnEnable()
    {
        StartCoroutine(GameStartCountdown());
    }    

    IEnumerator GameStartCountdown()
    {
        double startTime = GameManager.Instance.GameSettings.GameStartTime;
        double timeLeft = 10;

        while (timeLeft > 0)
        {
            timeLeft = startTime - PhotonNetwork.Time;
            countdownTMP.text = ((int)timeLeft).ToString();
            yield return new WaitForSecondsRealtime(1);
        }

        GameMenuManager.Instance.CloseMenu();
    }
}
