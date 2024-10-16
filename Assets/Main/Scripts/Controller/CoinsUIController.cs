using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsUIController : MonoBehaviour
{
    private const float SCALE_TIME = 0.5F;
    private const float SCALE_IN_TIME = 0.3F;
    private const float SCALE_BACK_TIME = 0.5F;
    private const float SCALE_MAX = 0.6F;
    private const float TRANSLATION_TIME = 1.4F;

    public int amount = 0;
    public Text text;
    public RectTransform[] flyingImages;
    private int currentIndex = 0;
    public RectTransform canvas;
    private void Awake()
    {
        EventManager.StartListening(EventName.UPDATE_GAME_COINS_TEXT, UpdateText);
        UpdateText();
    }
    public void AddCoins(int addedCoins, Vector3 worldPosition)
    {
        if (addedCoins > 0)
        {
            currentIndex = (currentIndex + 1) % flyingImages.Length;
            amount += addedCoins;
            CurrentPlaySingleton.GetInstance().coins = amount;
            Vector3 initialPos = Camera.main.WorldToViewportPoint(worldPosition);
            Vector2 initialPosAnchored = initialPos.x * canvas.rect.width * Vector3.right + initialPos.y * canvas.rect.height * Vector3.up;

            flyingImages[currentIndex].transform.localScale = Vector3.zero;
            LeanTween.scale(flyingImages[currentIndex], Vector3.one * SCALE_MAX*2, SCALE_IN_TIME).setEaseInOutBounce();
            LeanTween.scale(flyingImages[currentIndex], Vector3.one * SCALE_MAX, SCALE_BACK_TIME).setEaseOutBack().setDelay(SCALE_IN_TIME);
            flyingImages[currentIndex].anchoredPosition = initialPosAnchored;

            LeanTween.move(flyingImages[currentIndex].gameObject, transform.position, TRANSLATION_TIME).setOnComplete(OnArrive).setEaseInExpo();
            LeanTween.scale(flyingImages[currentIndex], Vector3.zero, SCALE_TIME).setDelay(TRANSLATION_TIME).setEaseInBack();
            EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("tin"));

        }
    }
    private void OnArrive()
    {
        UpdateText();
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("coins"));
    }

    private void UpdateText(EventData arg0 = null)
    {
        amount = CurrentPlaySingleton.GetInstance().coins;
        text.text = amount.ToString();
    }
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.UPDATE_GAME_COINS_TEXT, UpdateText);
    }
}
