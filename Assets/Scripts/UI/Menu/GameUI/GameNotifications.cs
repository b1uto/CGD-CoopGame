using CGD.Case;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;
using TMPro;
using UnityEngine;
using System.Linq;
using CGD.Networking;


namespace CGD.Gameplay
{
    public class GameNotifications : MonoBehaviour
    {
        /// <summary>
        /// duration of notification on screen
        /// </summary>
        [SerializeField] private float notifTime;

        /// <summary>
        /// display text
        /// </summary>
        [SerializeField] private CanvasGroup notifCanvasGroup;
        
        /// <summary>
        /// display text
        /// </summary>
        [SerializeField] private TextMeshProUGUI tmp;

        private void Awake()
        {
            NetworkEvents.OnPlayerSubmittedClue += OnPlayerSubmittedClue;
            NetworkEvents.OnPlayerSharedTeamClue += OnPlayerSharedClue;
        }
        private void OnDestroy()
        {
            NetworkEvents.OnPlayerSubmittedClue -= OnPlayerSubmittedClue;
            NetworkEvents.OnPlayerSharedTeamClue -= OnPlayerSharedClue;

        }
        #region Network Events
        private void OnPlayerSubmittedClue(string id, int actorNumber, bool analysed)
        {
            if (PhotonNetwork.CurrentRoom.Players.TryGetValue(actorNumber, out Player player))
            {
                string txt = player.NickName.Substring(0, 12) + " submitted clue";
                ShowNotification(null, txt);
            }
        }
        private void OnPlayerSharedClue(string id, int actorNumber, bool analysed)
        {
            if (PhotonNetwork.CurrentRoom.Players.TryGetValue(actorNumber, out Player player))
            {
                string txt = player.NickName.Substring(0, 12) + " shared clue";
                ShowNotification(null, txt);
            }
        }
        #endregion


        #region Main
        private void ShowNotification(Sprite icon, string text) 
        {
            notifCanvasGroup.alpha = 0;
            tmp.text = text;

            var seq = DOTween.Sequence();
            seq.Append(notifCanvasGroup.DOFade(1, 0.5f));
            seq.AppendInterval(notifTime);
            seq.Append(notifCanvasGroup.DOFade(0, 0.5f));
        }
        #endregion


    }
}
