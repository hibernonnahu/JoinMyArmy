
using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEnemyLocalLevel : MonoBehaviour
{

    public Text levelText;
    public RectTransform levelTextContainer;
    public Text priceText;

    public Button button;
    public string asset = "";
    public Image image;
    public int baseCost;
    public int id;
    private int price;
    private int level;
   

    // Start is called before the first frame update
    void Start()
    {
        name = asset;
        image.sprite = Resources.Load<Sprite>("CharacterIcons/" + name);
        if (image.sprite == null)
        {
            image.gameObject.SetActive(false);
        }
        level = SaveData.GetInstance().GetEnemyLocalLevel(id);
        CalculateCost();
        EventManager.StartListening(EventName.UPDATE_COINS_TEXT, CalculateCost);
 
    }

    public void OnClick()
    {
        SaveData.GetInstance().coins -= price;
        level = SaveData.GetInstance().AddEnemyLocalLevel(id);
        EventManager.TriggerEvent(EventName.UPDATE_COINS_TEXT);
        EventManager.TriggerEvent(EventName.SHAKE_CAM_POS, EventManager.Instance.GetEventData().SetFloat(0.2f));
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("pum"));
    }


    private void CalculateCost(EventData arg0 = null)
    {
        price = (int)(baseCost + baseCost * level * 0.90f);
        button.interactable = SaveData.GetInstance().coins >= price;
        UpdateText();
    }

    private void UpdateText()
    {
        priceText.text = price.ToString("00000");
        levelText.text = "Level: " + level.ToString();
    }
 
    private void OnDestroy()
    {
        EventManager.StopListening(EventName.UPDATE_COINS_TEXT, CalculateCost);
    }
}
