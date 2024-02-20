using BrunoMikoski.AnimationSequencer;
using UnityEngine;
using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour, IOnEventCallback
{
    [SerializeField] private string[] levelPrompts;
    
    [SerializeField] private ScrollingText loadingLabel;

    private AnimationSequencerController animSeqController;

    private int sceneIndex = 0;
    private bool loadScene = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        animSeqController = GetComponent<AnimationSequencerController>();
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void FadeIn() => animSeqController.PlayForward();
    private void FadeOut() => animSeqController.PlayBackwards();


    public void FinishLoad() 
    {
        if (PhotonNetwork.IsMasterClient && loadScene) 
        {
            loadScene = false;
            PhotonNetwork.LoadLevel(sceneIndex);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == GameProperties.LoadingLevelEvent)
        {
           sceneIndex = (int)photonEvent.CustomData;

            if (sceneIndex < 0 || sceneIndex >= levelPrompts.Length)
                loadingLabel.SetMessage("");
            else
                loadingLabel.SetMessage(levelPrompts[sceneIndex]);

            loadScene = true;
            FadeIn();
        }

        if (eventCode == GameProperties.AllPlayersLoadedEvent)
        {
            FadeOut();
        }
    }
}
