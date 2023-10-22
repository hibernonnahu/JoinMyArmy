using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerDispatch : MonoBehaviour
{
    public string eventString = "";

    private bool enable = true;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (enable)
        {
            enable = false;
            EventManager.TriggerEvent(eventString);
        }
    }
}
