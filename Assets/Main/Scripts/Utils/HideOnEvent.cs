using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(EventName.HIDE_GAMEOBJECT, OnHide);
    }

    private void OnHide(EventData arg0)
    {
        gameObject.SetActive(!arg0.boolData);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.HIDE_GAMEOBJECT, OnHide);

    }
}
