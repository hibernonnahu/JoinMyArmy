using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnCustomEvent : MonoBehaviour
{
    public string code = "";
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(code, OnShow);
    }

    private void OnShow(EventData arg0)
    {
        gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(code, OnShow);

    }
}
