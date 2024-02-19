using System.Collections;
using TMPro;
using UnityEngine;


public class ScrollingText : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]private string message;

    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        StartCoroutine(TypeText());
    }
    IEnumerator TypeText()
    {
        if (tmp == null)
            tmp = GetComponent<TextMeshProUGUI>();
        
        tmp.text = "";

        foreach (char letter in message.ToCharArray())
        {
            tmp.text += letter; 
            yield return new WaitForSeconds(speed);
        }
    }
}
