using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;



namespace CGD.Gameplay
{
    public class ClueObject : Pickup
    {
        public CGD.Case.Clue clue;

        private int viewId;

        private void Awake()
        {
            MiniGameManager.OnMiniGameFinished += MiniGameFinishedCallback;
        }
        private void OnDestroy()
        {
            MiniGameManager.OnMiniGameFinished -= MiniGameFinishedCallback;
        }

        public override void Interact(int viewId)
        {
            if(clue.analyseTool == Case.AnalyseTool.None) 
                photonView.RPC(nameof(CollectClue), RpcTarget.All, viewId);
            else 
            {
                this.viewId = viewId;
                MiniGameManager.Instance.LoadMiniGame(clue);
            }
        }

        private void MiniGameFinishedCallback(string id, bool status) 
        {
            if(id == clue.id) 
            {
                if(status == true)
                {
                    photonView.RPC(nameof(CollectClue), RpcTarget.All, viewId);
                }
                else 
                {
                    Interactable = false;
                }
            }
        }

        [PunRPC]
        private void CollectClue(int viewId)
        {
            var view = PhotonView.Find(viewId);

            if (view && view.TryGetComponent<PlayerManager>(out var player))
            {
                player.CollectClue(clue.id);
            }

            gameObject.SetActive(false);

        }
    }
}
