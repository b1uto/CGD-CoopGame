using CGD;
using Photon.Pun;
using UnityEngine;

public class Pickup : MonoBehaviourPunCallbacks, IInteractable
{
    [SerializeField] private GameObject interactionPrompt;

    private int clueId; //

    public bool Interactable { get { return true; } set { } }

    public void Interact(int viewId)
    {
        photonView.RPC(nameof(CollectClue), RpcTarget.MasterClient, viewId);
    }

    [PunRPC]
    private void CollectClue(int viewId) 
    {
        var view = PhotonView.Find(viewId);

        if (view && view.TryGetComponent<PlayerManager>(out var player)) 
        {
            player.GetClue(clueId);
            PhotonNetwork.Destroy(gameObject);
        }

    }
    public void OnExitFocus()
    {
        if(interactionPrompt) interactionPrompt.SetActive(false);
    }
    public void OnFocus()
    {
        if (interactionPrompt)
            interactionPrompt.SetActive(true);
    }
}
