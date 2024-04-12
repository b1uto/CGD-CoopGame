using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using PlayFab.ClientModels;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CGD.Gameplay
{
    public class VotePanel : MonoBehaviour, IOnEventCallback
    {
        [SerializeField] private GraphicRaycaster raycaster;

        [SerializeField] private VoteCard[] cards;
        [SerializeField] private TextMeshProUGUI solveTMP;

        #region PUN
        public virtual void OnEnable()
        {
            PhotonNetwork.AddCallbackTarget(this);
        }

        public virtual void OnDisable()
        {
            PhotonNetwork.RemoveCallbackTarget(this);
        }
        public void OnEvent(EventData photonEvent)
        {
            if(photonEvent.Code == GameSettings.PlayerSolvedCase) 
            {
                var data = (object[])photonEvent.CustomData;
                var actorNumber = (int)data[0];
                var solved = (bool)data[1];

                solveTMP.text = solved ? "Case Is Solved!" : "Wrong Solution!";
            }
        }
        #endregion

        public void Btn_SolveCase()
        {
            var items = cards.Select(x => x.GetItem()).ToArray();
            solveTMP.text = "...";

            BoardRoundManager.Instance.SubmitSolution(PhotonNetwork.LocalPlayer.ActorNumber, items);
            raycaster.enabled = false;
        }

    }
}