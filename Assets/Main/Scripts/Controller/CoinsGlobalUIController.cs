using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsGlobalUIController : MonoBehaviour
{

    public Text text;

    private void Awake()
    {
        EventManager.StartListening(EventName.UPDATE_COINS_TEXT, OnUpdateText);
        OnUpdateText(null);
    }

    private void OnUpdateText(EventData arg0)
    {
        text.text = SaveData.GetInstance().coins.ToString("0000000");
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.UPDATE_COINS_TEXT, OnUpdateText);

    }
}
