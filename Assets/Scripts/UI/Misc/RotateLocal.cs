using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLocal : MonoBehaviour
{
    [SerializeField] private float speed;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectTransform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
