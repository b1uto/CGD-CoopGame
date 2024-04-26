using Photon.Pun;
using ExitGames.Client.Photon;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Collections;
using UnityEngine.UI;
using CGD.Gameplay;
using UnityEngine;
using System.Diagnostics.Tracing;
using CGD.Networking;

public class SceneLoader : MonoBehaviour
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
            NetworkEvents.OnLoadScene += OnLoadScene;
            NetworkEvents.OnAllPlayersLoaded += OnAllPlayersLoaded;
        }
    }
    private void OnDestroy()
    {
        RequestLoadScene -= LoadScene;
        NetworkEvents.OnLoadScene -= OnLoadScene;
        NetworkEvents.OnAllPlayersLoaded -= OnAllPlayersLoaded;
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

    #region Callbacks
    private void OnLoadScene(int index) => LoadScene(index, true);
    private void OnAllPlayersLoaded(double networkTime) => StartCoroutine(DelayFadeOut());
    #endregion
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
