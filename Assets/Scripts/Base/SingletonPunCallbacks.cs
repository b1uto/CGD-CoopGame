using Photon.Pun;
using UnityEngine;

public class SingletonPunCallbacks<T> : MonoBehaviourPunCallbacks where T : Component
{
    public static T Instance { get { return _instance; } }

    [Header("Singleton Settings")]
    /// <summary>
    /// Toggle whether to destroy this object when loading between scenes.
    /// </summary>
    [SerializeField] private bool _persistant;
    private static T _instance;

    protected virtual void Awake()
    {
        if (Instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            _instance = this as T;
            if (_persistant)
                DontDestroyOnLoad(gameObject);
        }
    }

}
