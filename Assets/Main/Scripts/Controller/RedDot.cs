using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedDot : MonoBehaviour
{
    private Image redDot;
    private string code;
    public void DotActivate(float x, float y, string code)
    {
        this.code = code;
        if (redDot == null)
        {
            redDot = Utils.CreateRedDot(transform, redDot);
            redDot.transform.position += Vector3.right * x + Vector3.up * y;
            redDot.gameObject.SetActive(false);
            GetComponent<Button>().onClick.AddListener(() => { redDot.gameObject.SetActive(false); });
        }
        EventManager.StartListening(EventName.MENU_BUTTON + code, ActivateRedDot);



    }

    private void ActivateRedDot(EventData arg0)
    {
        var bhb = GetComponent<BottomHudButton>();
        if (bhb != null)
        {
            if (!bhb.isActive)
                redDot.gameObject.SetActive(true);
        }
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.MENU_BUTTON + code, ActivateRedDot);
    }
}
