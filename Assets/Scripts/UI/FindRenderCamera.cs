using CGD.Gameplay;
using System.Collections;
using UnityEngine;

public class FindRenderCamera : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private void Start()
    {
        StartCoroutine(FindPlayerCamera());
    }

    IEnumerator FindPlayerCamera() 
    {
        while (PlayerManager.LocalPlayerInstance == null) 
        {
            yield return new WaitForEndOfFrame();
        }

        canvas.worldCamera = PlayerManager.LocalPlayerInstance.GetComponentInChildren<Camera>();

    }

}
