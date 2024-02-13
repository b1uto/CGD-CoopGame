using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugCanvas : Singleton<DebugCanvas>
{
    [SerializeField] private TextMeshProUGUI inputTMP;
    [SerializeField] private TextMeshProUGUI consoleTMP;


    private Queue<string> msgs = new Queue<string>();

    private float displayTime = 4.0f, timer = 0;

    private void Update()
    {
        timer += Time.deltaTime;
        if(timer > displayTime) 
        {
            timer = 0;  

            if(msgs.Count > 0) 
            {
                msgs.Dequeue(); 

                consoleTMP.text = (msgs.Count > 0) ? PrintQueue() : "";
            }
        }
    }

    private string PrintQueue() 
    {
        string str = "";

        foreach(var strng in  msgs) 
        {
            str += strng;
        }
        return str;
    }

    public void AddConsoleLog(string msg) 
    {
        msgs.Enqueue(msg+"\n");
        consoleTMP.text = PrintQueue();
    }
    public void OverrideConsoleLog(string msg)
    {
        consoleTMP.text = msg;
    }



    public void UpdateInputText(string text)
    {
       inputTMP.text = text;
    }



}
