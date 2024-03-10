using CGD;
using CGD.Extensions;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardPanel : MenuPanel
{
    [SerializeField] private Image turnTimerImg;
    [SerializeField] private TextMeshProUGUI[] turnIndicators;
    [SerializeField] private TextMeshProUGUI turnPromptTMP;

    private Coroutine turnTimerCoroutine;

    private void OnEnable()
    {
        UpdateTurnIndicators();
    }

    private void Start()
    {
        EvidenceBoard.OnNextPlayerTurn += NextPlayersTurn;
    }
    private void OnDestroy()
    {
        EvidenceBoard.OnNextPlayerTurn -= NextPlayersTurn;
    }

    private void NextPlayersTurn(Player player, int activePlayer) 
    {
        UpdateTurnIndicators(activePlayer);
        UpdateTurnPrompt(player);
        CoroutineUtilities.StartExclusiveCoroutine(TurnTimer(), ref turnTimerCoroutine, this);
    }

    private void UpdateTurnIndicators(int activePlayer = 0) 
    {
        int numOfPlayers = PhotonNetwork.PlayerList.Length;
    
        for(int i = 0; i < turnIndicators.Length; i++) 
        {
            turnIndicators[i].gameObject.SetActive(i < numOfPlayers);

            Color targetCol = activePlayer == i ? new Color(1f, 0.5f, 0f) : Color.white;
            turnIndicators[i].DOColor(targetCol, 1.0f); //TODO change to analog flicker.
        }
    }

    private void UpdateTurnPrompt(Player player) 
    {
        if (PhotonNetwork.LocalPlayer == player)
            turnPromptTMP.text = "It's Your Turn!";
        else
            turnPromptTMP.text = "<color=orange> Player Is Making A Decision </color>";
    }

    IEnumerator TurnTimer()
    {
        var turnTime = GameManager.Instance.GameSettings.TurnTime;
        var turnEndTime = PhotonNetwork.Time + turnTime;
        

        while(PhotonNetwork.Time < turnEndTime) 
        {
            var timeLeft = turnEndTime - PhotonNetwork.Time;

            turnTimerImg.fillAmount = (float)(timeLeft / turnTime);

            yield return new WaitForEndOfFrame();
        }


    }



}
