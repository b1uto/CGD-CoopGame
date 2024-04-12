using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using CGD.Gameplay;
using UnityEngine;

public class SceneLoader : MonoBehaviour, IOnEventCallback
{
#if UNITY_EDITOR
    public InspectorButton show = new InspectorButton("FadeIn");
    public InspectorButton hide = new InspectorButton("FadeOut");
#endif
    
    [SerializeField] private string[] levelPrompts;

    [Header("Tween References")]
    [SerializeField] private GameObject canvas;
    [SerializeField] private Image backgroundImg;
    [SerializeField] private ScrollingText loadingTMP;


    public delegate void LoadSceneDelegate(int index, bool synced);
    public static LoadSceneDelegate RequestLoadScene;

    private void Awake()
    {
        if (FindObjectsOfType<SceneLoader>().Length > 1)
        {
            Destroy(gameObject);
        }
        else 
        {
            DontDestroyOnLoad(gameObject);  
            RequestLoadScene += LoadScene;
        }
    }

    private void OnDestroy()
    {
        RequestLoadScene -= LoadScene;
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    private void FadeIn(int sceneIndex, bool synced) //=> animSeqController.PlayForward();
    {
        canvas.gameObject.SetActive(true);
        backgroundImg.color = Color.clear;
        loadingTMP.gameObject.SetActive(false);


        var seq = DOTween.Sequence();
        seq.Append(backgroundImg.DOFade(1, 0.5f));
        seq.OnComplete(() =>
        {
            loadingTMP.gameObject.SetActive(true);
            FinishLoad(sceneIndex, synced);
        });
    }
    private void FadeOut()/// => animSeqController.PlayBackwards();
    {
        loadingTMP.gameObject.SetActive(false);

        var seq = DOTween.Sequence();
        seq.Append(backgroundImg.DOFade(0, 1.0f));
        seq.OnComplete(() =>
        {
            canvas.gameObject.SetActive(false);
        });
    }

    public void FinishLoad(int sceneIndex, bool synced) 
    {
        if (synced && PhotonNetwork.IsMasterClient) 
        {
            PhotonNetwork.LoadLevel(sceneIndex);
        }
        else if(!synced) 
        {
            SceneManager.LoadScene(sceneIndex);
            Invoke("FadeOut", 1);
        }
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;

        if (eventCode == GameSettings.PunLoadScene)
        {
            RequestLoadScene?.Invoke((int)photonEvent.CustomData, true);
        }

        if (eventCode == GameSettings.PunAllPlayersLoaded)
        {
            StartCoroutine(DelayFadeOut());
        }
    }

    private void LoadScene(int sceneIndex, bool synced) 
    {
        if (sceneIndex < 0 || sceneIndex >= levelPrompts.Length)
            loadingTMP.SetMessage("");
        else
            loadingTMP.SetMessage(levelPrompts[sceneIndex]);


        FadeIn(sceneIndex, synced);
    }

    IEnumerator DelayFadeOut() 
    {
        yield return new WaitForEndOfFrame();

        var delay = GameManager.Instance.GameSettings.GameStartTime - PhotonNetwork.Time;

        yield return new WaitForSecondsRealtime((float)delay + 1);

        FadeOut();
    }
}
