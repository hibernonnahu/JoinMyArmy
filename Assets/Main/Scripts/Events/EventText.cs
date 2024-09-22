using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventText : MonoBehaviour
{
    protected  float TEXT_DELAY = 5f;
    protected Text text;
    protected List<string> queue = new List<string>();
    protected string code;
    // Start is called before the first frame update
    private void Awake()
    {
        code = EventName.MAIN_TEXT;

    }
    public virtual void Start()
    {
        text = GetComponentInChildren<Text>();
        EventManager.StartListening(code, PlayText);
    }

    protected virtual void PlayText(EventData arg0)
    {
        if (arg0.stringData == "")
        {
            LeanTween.cancel(text.gameObject);
            queue.Clear();
            text.text = "";
        }
        else
        {
            queue.Add(arg0.stringData);
            if (queue.Count == 1)
            {
                DisplayText();
            }
        }
    }

    private void DisplayText()
    {
        text.text = queue[0];
        LeanTween.delayedCall(text.gameObject,TEXT_DELAY, RemoveFirst).setIgnoreTimeScale(true);
    }

    protected virtual void RemoveFirst()
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
        LeanTween.cancel(text.gameObject);
        EventManager.StopListening(code, PlayText);
    }
}
