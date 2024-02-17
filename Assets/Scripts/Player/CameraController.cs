using CGD;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private PlayerController controller;


    [Range(0.001f, 0.01f)]
    [SerializeField]
    private float amount = 0.002f;


    [Range(1f, 30f)]
    [SerializeField]
    private float frequency = 10.0f;


    [Range(10f, 100f)]
    [SerializeField]
    private float smooth = 10.0f;

    /// <summary>
    /// Current local position of camera
    /// </summary>
    private Vector3 position;


    private Vector3 startPosition = new Vector3(0, 1.5f, 0);
    private void Update()
    {
        if (controller.GroundVelocity != Vector2.zero)
            HeadBob();
        else
            ResetPosition();
    }


    private void HeadBob()
    {
        position = Vector3.zero;
        position.y += Mathf.Lerp(position.y, Mathf.Sin(Time.time * frequency) * amount * 1.4f, smooth * Time.deltaTime);
        position.x += Mathf.Lerp(position.x, Mathf.Sin(Time.time * frequency /2f) * amount * 1.6f, smooth * Time.deltaTime);
        transform.localPosition += position;
    }

    private void ResetPosition() 
    {
        if(transform.localPosition != startPosition) 
            transform.localPosition = Vector3.Lerp(transform.localPosition, startPosition, Time.deltaTime);
    }


}
