using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEvent : MonoBehaviour
{
    public int v;
    // Start is called before the first frame update
    public void Trigger(string e)
    {
        EventManager.TriggerEvent(e,EventManager.Instance.GetEventData().SetInt(v));
    }
}
