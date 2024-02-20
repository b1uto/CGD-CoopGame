using System.Collections;
using TMPro;
using UnityEngine;


public class ScrollingText : MonoBehaviour
{
    [SerializeField]private float speed;
    [SerializeField]private string message;
    [SerializeField] private GameObject goToEnable;

    private TextMeshProUGUI tmp;

    private void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        StartCoroutine(TypeText());
    }

    public void SetMessage(string msg) => message = msg;
    IEnumerator TypeText()
    {
        if (tmp == null)
            tmp = GetComponent<TextMeshProUGUI>();

        if (goToEnable != null)
            goToEnable.SetActive(false);
        
        tmp.text = "";

        foreach (char letter in message.ToCharArray())
        {
            tmp.text += letter; 
            yield return new WaitForSeconds(speed);
        }

        if(goToEnable != null) 
        {
            yield return new WaitForSeconds(1);
            goToEnable.SetActive(true);
        }
    }
}
