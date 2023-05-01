using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventText : MonoBehaviour
{
    private const float TEXT_DELAY = 3f;
    private Text text;
    private List<string> queue = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponentInChildren<Text>();
        EventManager.StartListening(EventName.MAIN_TEXT, PlayText);
    }

    private void PlayText(EventData arg0)
    {
        queue.Add(arg0.stringData);
        if (queue.Count == 1)
        {
            DisplayText();
        }
    }

    private void DisplayText()
    {
        text.text = queue[0];
        LeanTween.delayedCall(TEXT_DELAY, RemoveFirst);
    }

    private void RemoveFirst()
    {
        queue.RemoveAt(0);
        if (queue.Count > 0)
        {
            DisplayText();
        }
        else
        {
            text.text = "";
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening("main text", PlayText);
    }
}
