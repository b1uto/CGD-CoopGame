using CGD;
using CGD.Case;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CGD.Utilities.ObjectPool<ClueCard>))]
public class CardDrawer : MonoBehaviour
{
    [SerializeField] private RectTransform submitButtonsRT;

    private CGD.Utilities.ObjectPool<ClueCard> objectPool;

    private RectTransform focusedCardRT;
    private string focusedCardID;

    private void Awake()
    {
        objectPool = GetComponent<CGD.Utilities.ObjectPool<ClueCard>>();
    }
    private void Start()
    {
        BoardRoundManager.OnNextPlayerTurn += NextPlayersTurn;
    }
    private void OnDestroy()
    {
        BoardRoundManager.OnNextPlayerTurn -= NextPlayersTurn;
    }

    private void NextPlayersTurn(Player player, int activePlayer)
    {
        gameObject.SetActive(player == PhotonNetwork.LocalPlayer);

        if (player == PhotonNetwork.LocalPlayer)
            DrawPlayerHand(ref GameManager.Instance.LocalPlayerManager.Clues);
    }

    public void DrawPlayerHand(ref Dictionary<string, ClueInfo> clues) 
    {
        foreach(var clue in clues) 
        {
            if (clue.Value.communityClue) 
                continue;

            var card = objectPool.GetObject();
            card.DrawCard(clue.Key, clue.Value.status == ClueStatus.Analysed, ShowButtons);
        }
    }

    private void ShowButtons(RectTransform rect, string id) 
    {
        focusedCardRT = rect;
        focusedCardID = id;

        var anchoredPos = submitButtonsRT.anchoredPosition;
        anchoredPos.x = rect.anchoredPosition.x;
        submitButtonsRT.anchoredPosition = anchoredPos;

        submitButtonsRT.gameObject.SetActive(true);
    }

    public void Btn_HideButtons()=> submitButtonsRT.gameObject.SetActive(false);

    public void Btn_Submit() => GameManager.Instance.LocalPlayerManager.SubmitClue(focusedCardID);

}
