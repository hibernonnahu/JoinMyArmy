using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonUpgradeStatsUI : MonoBehaviour
{
    public Text levelText;
    public Text priceText;
    public Text nameText;
    public RectTransform levelTextContainer;
    private Image image;
    public string skillPath;
    public string skillName;
    private float initialSize;
    private int level = 0;
    public int baseCost = 1;
    public int costCap = 1000;
    private Button button;
    private int price;
    private string key;
    private SQLManager sqlManager;
    private void Awake()
    {
        initialSize = levelTextContainer.transform.localScale.x;
        
    }
    void Start()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Skills/" + skillPath);
        nameText.text = skillName;
        key = SaveDataKey.STATS + "_" + CurrentPlaySingleton.GetInstance().book + "_" + skillName;
        level = SaveData.GetInstance().GetValue(key);
        CalculateCost();
        UpdateText();
        EventManager.StartListening(EventName.UPDATE_COINS_TEXT, CalculateCost);
    }

    // Update is called once per frame
    public void OnClick()
    {
        SaveData.GetInstance().coins -= price;
        level++;
        SaveData.GetInstance().Save(key, level);
        EventManager.TriggerEvent(EventName.UPDATE_COINS_TEXT, EventManager.Instance.GetEventData().SetInt(0));
        UpdateTextAnimation();
        EventManager.TriggerEvent(EventName.SHAKE_CAM_POS, EventManager.Instance.GetEventData().SetFloat(0.2f));
        EventManager.TriggerEvent(EventName.PLAY_FX, EventManager.Instance.GetEventData().SetString("pum"));
        EventManager.TriggerEvent(EventName.TUTORIAL_END, EventManager.Instance.GetEventData().SetInt(4));
        EventManager.TriggerEvent(EventName.SAVE_USER_DELAY);
    }
    private void UpdateText()
    {
        //cost
        levelText.text = "Level: " + level.ToString();
        priceText.text = price.ToString();
    }
    private void CalculateCost(EventData arg0 = null)
    {
        price = (int)(baseCost + baseCost * level * 0.90f);
        if (price > costCap)
        {
            price = costCap;
        }

        int currentChapter = SaveData.GetInstance().GetValue(SaveDataKey.CURRENT_BOOK_CHAPTER + CurrentPlaySingleton.GetInstance().book, CurrentPlaySingleton.GetInstance().initialChapter);
        button.interactable = SaveData.GetInstance().coins >= price && level < currentChapter;
        if (!button.interactable)
        {
           GetComponent<Pulse>().Pause(true);
        }
        else
        {
            GetComponent<Pulse>().Pause(false);
            EventManager.TriggerEvent(EventName.MENU_BUTTON + "STATS");
        }
    }
    private void UpdateTextAnimation()
    {
        LeanTween.cancel(levelTextContainer.gameObject);

        LeanTween.scale(levelTextContainer.gameObject, Vector3.one * initialSize * 1.5f, 0.3f).setEaseLinear().setOnComplete(
           () =>
           {
               LeanTween.scale(levelTextContainer.gameObject, Vector3.one * initialSize, 0.7f).setEaseOutBounce();
           }
           );
        UpdateText();

    }

    private void OnDestroy()
    {
        EventManager.StopListening(EventName.UPDATE_COINS_TEXT, CalculateCost);
    }
}
