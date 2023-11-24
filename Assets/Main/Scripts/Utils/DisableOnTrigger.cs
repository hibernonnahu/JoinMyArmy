using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnTrigger : MonoBehaviour
{
    public string eventName;
  

    private void Start()
    {
        EventManager.StartListening(eventName, OnTrigger);
    }
    protected  void OnTrigger(EventData arg0)
    {
        gameObject.SetActive(false);

    }
    private void OnDestroy()
    {
        EventManager.StopListening(eventName, OnTrigger);

    }
}
