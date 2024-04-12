using CGD.Case;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;


namespace CGD.Gameplay
{
    public class CardDrawer : MonoBehaviour
    {
        [SerializeField] private RectTransform submitButtonsRT;

        [SerializeField] private ClueCardPool objectPool;

        private string focusedCardID;
       
        public void Awake()
        {
            BoardRoundManager.OnNextPlayerTurn += OnNextPlayersTurn;
        }

        public void OnDestroy()
        {
            BoardRoundManager.OnNextPlayerTurn -= OnNextPlayersTurn;
        }

        public void OnNextPlayersTurn(int actorNumber)
        {
            //gameObject.SetActive(actorNumber == PhotonNetwork.LocalPlayer.ActorNumber);

            if (actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                DrawPlayerHand(ref GameManager.Instance.LocalPlayerManager.Clues);
        }

        public void DrawPlayerHand(ref Dictionary<string, ClueInfo> clues)
        {
            int lastIndex = 0;

            foreach (var clue in clues)
            {
                if (clue.Value.communityClue)
                    continue;

                var card = objectPool.GetObject();
                card.DrawCard(clue.Key, clue.Value.status == ClueStatus.Analysed, ShowButtons);
                lastIndex++;
            }

            if(lastIndex > 0)
                objectPool.ResetUnusedObjects(lastIndex);
        }

        private void ShowButtons(RectTransform rect, string id)
        {
            focusedCardID = id;

            var anchoredPos = submitButtonsRT.anchoredPosition;
            anchoredPos.x = rect.anchoredPosition.x;
            submitButtonsRT.anchoredPosition = anchoredPos;

            submitButtonsRT.gameObject.SetActive(true);
        }

        public void Btn_HideButtons() => submitButtonsRT.gameObject.SetActive(false);

        public void Btn_Submit() => GameManager.Instance.LocalPlayerManager.SubmitClue(focusedCardID);

    }
}