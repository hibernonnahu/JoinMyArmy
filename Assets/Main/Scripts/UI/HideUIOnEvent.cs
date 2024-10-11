using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUIOnEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(EventName.HIDE_CHARACTER_UI, onHide);

    }
    private void onHide(EventData arg0)
    {
        gameObject.SetActive(!arg0.boolData);
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.HIDE_CHARACTER_UI, onHide);

    }
}
