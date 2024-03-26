using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;



namespace CGD.Gameplay
{
    public class ClueObject : Pickup
    {
        public CGD.Case.Clue clue;

        /// <summary>
        /// Id to Clue Scriptable Object.
        /// </summary>
        //private string key;

        public override void Interact(int viewId)
        {
            photonView.RPC(nameof(CollectClue), RpcTarget.All, viewId);
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
