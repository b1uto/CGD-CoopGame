
using Photon.Pun;
using UnityEngine;

namespace CGD.Gameplay
{

    public class Pickup : MonoBehaviourPunCallbacks, IInteractable
    {
        [SerializeField] private GameObject interactionPrompt;

        private bool interactable = true;

        public bool Interactable { get { return interactable; } set { interactable = value; } } //Interactable with correct tool use.

        public virtual void Interact(int viewId)
        {
            //photonView.RPC(nameof(CollectClue), RpcTarget.MasterClient, viewId);
        }

       
        public void OnExitFocus()
        {
            if (interactionPrompt) interactionPrompt.SetActive(false);
        }
        public void OnFocus()
        {
            if (interactionPrompt)
                interactionPrompt.SetActive(true);
        }
    }
}
