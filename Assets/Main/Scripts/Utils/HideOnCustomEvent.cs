using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnCustomEvent : MonoBehaviour
{
    public string code = "";
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(code, OnHide);
    }

    private void OnHide(EventData arg0)
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(code, OnHide);

    }
}
