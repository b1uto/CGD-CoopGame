using CGD.Networking;
using CGD.Gameplay;
using Photon.Pun;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CGD.UI
{
    public class VotePanel : MonoBehaviour
    {
        [SerializeField] private GraphicRaycaster raycaster;

        [SerializeField] private VoteCard[] cards;
        [SerializeField] private TextMeshProUGUI solveTMP;

        #region MonoBehaviour
        private void Awake()
        {
            NetworkEvents.OnPlayerSolvedCase += OnPlayerSolvedCase;
        }
        private void OnDestroy()
        {
            NetworkEvents.OnPlayerSolvedCase -= OnPlayerSolvedCase;
        }
        #endregion
        private void OnPlayerSolvedCase(int actorNumber, bool solved) 
        {
            solveTMP.text = solved ? "Case Is Solved!" : "Wrong Solution!";
        }

        public void Btn_SolveCase()
        {
            var items = cards.Select(x => x.GetItem()).ToArray();
            solveTMP.text = "...";

            BoardRoundManager.Instance.SubmitSolution(PhotonNetwork.LocalPlayer.ActorNumber, items);
            raycaster.enabled = false;
        }

    }
}