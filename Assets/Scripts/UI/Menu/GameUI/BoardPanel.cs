using CGD;
using CGD.Gameplay;
using CGD.Extensions;
using DG.Tweening;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardPanel : MenuPanel
{
    [SerializeField] private Image turnTimerImg;
    [SerializeField] private PlayerCard[] boardPlayers;
    [SerializeField] private TextMeshProUGUI turnPromptTMP;
    [SerializeField] private TextMeshProUGUI movesLeftTMP;
    [SerializeField] private Toggle panelToggle;
    [SerializeField] private RectTransform communityCardContainer;
    [SerializeField] private RectTransform playerBar;


    [SerializeField] private GameObject clueCardPrefab;
    [SerializeField] private GameObject boardPanel;
    [SerializeField] private GameObject casePanel;
    [SerializeField] private GameObject playerBarBlocker;

    private Coroutine turnTimerCoroutine;
    
    private BoardRoundManager BRM { get { return BoardRoundManager.Instance; } }    


    public void Awake()
    {
        BoardRoundManager.OnNextPlayerTurn += NextPlayersTurn;
        BoardRoundManager.OnPlayerSkippedTurn += SkippedTurnPrompt;
        BoardRoundManager.OnClueSubmitted += AnimateCardSubmission;
        BoardRoundManager.OnMoveMade += OnMoveMade;
    }
    public void OnDestroy()
    {
        BoardRoundManager.OnNextPlayerTurn -= NextPlayersTurn;
        BoardRoundManager.OnPlayerSkippedTurn -= SkippedTurnPrompt;
        BoardRoundManager.OnClueSubmitted -= AnimateCardSubmission;
        BoardRoundManager.OnMoveMade -= OnMoveMade;
    }

    public void OnEnable()
    {
        DrawBoardPlayers();
        panelToggle.isOn = true;
    }

    public void Start()
    {
        panelToggle.onValueChanged.AddListener(OnTogglePressed);
    }


    public void OnTogglePressed(bool isOn)
    {
        boardPanel.SetActive(isOn);
        casePanel.SetActive(!isOn);
    }

    private void DrawBoardPlayers()
    {
        if (BRM == null) 
            return;


        int index = 0;
        Player[] players = (BRM.PlayerList == null) 
            ? new Player[0] : BRM.PlayerList.Where(x => x.IsLocal == false).ToArray();


        foreach (var boardPlayer in boardPlayers)
        {
            if(index >= players.Length)  
            {
                boardPlayer.gameObject.SetActive(false);
            }
            else
            {
                var player = players[index++];

                boardPlayer.gameObject.SetActive(true);
                boardPlayer.Draw(player.NickName, 0, player.ActorNumber);
            }
        }
    }

    private void SkippedTurnPrompt(int actorNumber) => UpdateTurnPrompt(actorNumber, true);
    private void UpdateTurnPrompt(int actorNumber, bool skippedGo = false)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == actorNumber)
        {
            turnPromptTMP.text = skippedGo ? "Skipping Your Turn" : "It's Your Turn!";
        }
        else
        {
            var player = BRM.PlayerList.Single(x => x.ActorNumber == actorNumber);
            turnPromptTMP.text = skippedGo ? $"{player.NickName} Has Skipped Their Turn" 
                : $"<color=orange>{player.NickName} Is Making A Decision </color>";
        }

        turnPromptTMP.alpha = 0;
        turnPromptTMP.DOFade(1, 0.4f).OnComplete(() => turnPromptTMP.DOFade(0, 1.0f).SetDelay(3));
    }

    private void UpdateBoardPlayers(int actorNumber) 
    {
        foreach(var player  in boardPlayers) 
        {
            player.ToggleTurn(actorNumber);
        }
    }

    private void NextPlayersTurn(int actorNumber)
    {
        playerBar.gameObject.SetActive(actorNumber == PhotonNetwork.LocalPlayer.ActorNumber);
        UpdateTurnPrompt(actorNumber);
        UpdateBoardPlayers(actorNumber);
        CoroutineUtilities.StartExclusiveCoroutine(TurnTimer(), ref turnTimerCoroutine, this);
    }

    IEnumerator TurnTimer()
    {
        var turnTime = GameManager.Instance.GameSettings.TurnTime;
        var turnEndTime =   GameManager.Instance.GameSettings.TurnEndTime;

        while(PhotonNetwork.Time < turnEndTime) 
        {
            var timeLeft = turnEndTime - PhotonNetwork.Time;

            turnTimerImg.fillAmount = (float)(timeLeft / turnTime);

            yield return new WaitForEndOfFrame();
        }
    }

    private void AnimateCardSubmission(string id, int actorNumber, bool analysed)
    {
        //TODO get rid of linq, adjust for player submission
        var boardPlayer = PhotonNetwork.LocalPlayer.ActorNumber == actorNumber ? playerBar :
                boardPlayers.SingleOrDefault(x => x.ActorNumber == actorNumber).GetComponent<RectTransform>();

        playerBarBlocker.SetActive(true);

        var card = Instantiate(clueCardPrefab, transform).GetComponent<ClueCard>();
        card.DrawCard(id, analysed, null);

        var rect = card.GetComponent<RectTransform>();

        rect.position = boardPlayer.position;

        rect.DOMove(communityCardContainer.position, 1.5f).OnComplete(() =>
        {
            rect.SetParent(communityCardContainer);
            playerBarBlocker.SetActive(false);
        });
    }

    private void OnMoveMade(int movesLeft) 
    {
        movesLeftTMP.text = $"{movesLeft} Moves Left";
    }
}
