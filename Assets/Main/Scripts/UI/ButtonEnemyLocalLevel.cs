
using System;
using UnityEngine;
using UnityEngine.UI;

public class ButtonEnemyLocalLevel : MonoBehaviour
{

    public Text levelText;
    public RectTransform levelTextContainer;
    private float initialSize;
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
        initialSize = levelTextContainer.transform.localScale.x;
        name = asset;
        image.sprite = Resources.Load<Sprite>("CharacterIcons/" + name);
        if (image.sprite == null)
        {
            image.gameObject.SetActive(false);
        }
        level = SaveData.GetInstance().GetEnemyLocalLevel(id);
        CalculateCost();
        UpdateText();
        EventManager.StartListening(EventName.UPDATE_COINS_TEXT, CalculateCost);

    }

    public void OnClick()
    {
        SaveData.GetInstance().coins -= price;
        level = SaveData.GetInstance().AddEnemyLocalLevel(id);
        EventManager.TriggerEvent(EventName.UPDATE_COINS_TEXT);
        UpdateTextAnimation();
        EventManager.TriggerEvent(EventName.SHAKE_CAM_POS, EventManager.Instance.GetEventData().SetFloat(0.2f));
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("pum"));
        EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(4));
    }


    private void CalculateCost(EventData arg0 = null)
    {
        price = (int)(baseCost + baseCost * level * 0.90f);
        button.interactable = SaveData.GetInstance().coins >= price;
        if (!button.interactable)
        {
            Destroy(GetComponent<Pulse>());
        }
    }

    private void UpdateText()
    {
        levelText.text = "Level: " + level.ToString();
        priceText.text = price.ToString();
    }
    private void UpdateTextAnimation()
    {
        LeanTween.cancel(levelTextContainer.gameObject);
        levelText.text = "Level: " + level.ToString();
        LeanTween.scale(levelTextContainer.gameObject, Vector3.one * initialSize * 1.5f, 0.3f).setEaseLinear().setOnComplete(
           () =>
           {
               LeanTween.scale(levelTextContainer.gameObject, Vector3.one * initialSize, 0.7f).setEaseOutBounce();
           }
           );
        priceText.text = price.ToString();

    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.UPDATE_COINS_TEXT, CalculateCost);
    }
}
