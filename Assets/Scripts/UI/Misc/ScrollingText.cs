using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public class ScrollingText : MonoBehaviour
{
    public UnityEvent OnFinishedScrolling;
    public UnityEvent OnTurningOff;
    
    [SerializeField]private float speed;
    [SerializeField]private string message;
    [SerializeField]private bool animateElipsis;

    private TextMeshProUGUI textTMP;
    private Coroutine coroutine;


    private void Awake()
    {
        textTMP = GetComponent<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        StartCoroutine(TypeText());
    }
    private void OnDisable()
    {
        OnTurningOff?.Invoke();
        StopAllCoroutines();
    }

    public void SetMessage(string msg) => message = msg;
    IEnumerator TypeText()
    {
        if (textTMP == null)
            textTMP = GetComponent<TextMeshProUGUI>();

        var sb = new StringBuilder();

        foreach (char letter in message.ToCharArray())
        {
            sb.Append(letter);
            textTMP.text = sb.ToString();

            yield return new WaitForSecondsRealtime(speed);
        }

        OnFinishedScrolling?.Invoke();

        int dots = 0;
        
        while (animateElipsis) 
        {
            if (dots < 3)
            {
                dots++;
                sb.Append('.');
            }
            else 
            {
                dots = 0;
                sb.Length -= 3;
            }

            textTMP.text = sb.ToString();
            yield return new WaitForSecondsRealtime(speed);
        }


    }
}
