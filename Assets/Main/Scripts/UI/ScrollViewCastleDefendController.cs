using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewCastleDefendController : MonoBehaviour
{
    public MapLevelSelectController buttonPrefab;
    public string resourcesFolder = "";
    private GridLayoutGroup gridLayoutGroup;
    private RectTransform rect;
    float cellSpace;
    float lastValue;
    int repeatLastValue;
    bool enableScroll = true;
    float lastDestiny;
    int multiplier = 1;
    int currentCastle;
    MapLevelSelectController[] mapSelect;
    RectTransform hud;
    private void Start()
    {
        hud = GameObject.FindGameObjectWithTag("hud").GetComponent<RectTransform>();
        rect = transform.GetComponent<RectTransform>();
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
        var prefabs = Resources.LoadAll<Sprite>(resourcesFolder);
        int currentChapter = SaveData.GetInstance().GetValue(SaveDataKey.CURRENT_BOOK_CHAPTER + 1, CurrentPlaySingleton.GetInstance().GetInitialChapter(1));
        string maxCastleKey = SaveDataKey.CASTLE_DEFENSE_BOOK_CHAPTER_LEVEL_WIN + "_" + CurrentPlaySingleton.GetInstance().book;
        currentCastle = SaveData.GetInstance().GetValue(maxCastleKey, 1);
        int max = GameConfig.GetInstance().maxChapterEnable[CurrentPlaySingleton.GetInstance().book - 1];
        if (currentCastle > max)
        {
            currentCastle = max;
            CurrentPlaySingleton.GetInstance().animateTransition = false;
        }
        else
        {
            if (currentCastle <= currentChapter)
                EventManager.TriggerEvent(EventName.MENU_BUTTON + "COMBAT");
        }
        if (currentCastle > currentChapter)
        {
            currentCastle = currentChapter;
        }
        var sortMaps = new Sprite[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            sortMaps[int.Parse(prefabs[i].name) - 1] = prefabs[i];
        }
        mapSelect = new MapLevelSelectController[prefabs.Length];
        foreach (var item in sortMaps)
        {
            var buttonResourcesCreator = Instantiate<MapLevelSelectController>(buttonPrefab);
            buttonResourcesCreator.image.sprite = item;
            buttonResourcesCreator.id = int.Parse(item.name);
            buttonResourcesCreator.transform.SetParent(transform);
            buttonResourcesCreator.transform.localScale = Vector3.one;
            buttonResourcesCreator.button.interactable = buttonResourcesCreator.id <= currentCastle;
            mapSelect[buttonResourcesCreator.id - 1] = buttonResourcesCreator;
        }
        cellSpace = gridLayoutGroup.cellSize.x + gridLayoutGroup.spacing.x;

        if (CurrentPlaySingleton.GetInstance().animateTransition && SaveData.GetInstance().GetMetric(SaveDataKey.GAME_TYPE, "Campaign") == "CastleDefense")
        {
            hud.anchoredPosition = Vector2.zero;
            mapSelect[currentCastle - 1].button.interactable = false;
            CurrentPlaySingleton.GetInstance().animateTransition = false;
            rect.anchoredPosition = Vector2.left * cellSpace * (currentCastle - 2);
            enableScroll = false;
            lastDestiny = -cellSpace * (currentCastle - 1);
            LeanTween.moveX(rect, lastDestiny, 1.25f).setDelay(1).setOnComplete(OnNewChapterUnlock);

        }
        else
        {
            rect.anchoredPosition = Vector2.left * cellSpace * (currentCastle - 1);
        }
    }

    private void OnNewChapterUnlock()
    {
        rect.anchoredPosition = Vector2.right * lastDestiny;
        mapSelect[currentCastle - 1].UnlockAnimation(() => { EnableScroll(); /*mapSelect[currentChapter - 1].OnClick();*/ });
    }

    public void OnValueChange(Vector2 v)
    {
        if (enableScroll)
        {
            int t = Mathf.FloorToInt(v.x * 100);
            if (t == lastValue)
            {
                repeatLastValue++;
                if (repeatLastValue > 10)
                {
                    LeanTween.cancel(rect);
                    float destiny = GetDestinyPosition();
                    if (destiny != lastDestiny)
                    {
                        lastDestiny = destiny;
                        LeanTween.moveX(rect, destiny, 0.5f).setOnComplete(EnableScroll);
                        enableScroll = false;
                        repeatLastValue = 0;
                    }

                }

            }
            else
            {
                if (t > lastValue)
                {
                    multiplier = 1;
                }
                else
                {
                    multiplier = -1;
                }
                repeatLastValue = 0;
            }

            lastValue = t;
        }

    }

    private void EnableScroll()
    {
        rect.anchoredPosition = Vector2.right * lastDestiny;
        enableScroll = true;
        hud.anchoredPosition = Vector2.right * 9999;

    }

    private float GetDestinyPosition()
    {

        return Mathf.Round((rect.anchoredPosition.x - (cellSpace * 0.4f * multiplier)) / cellSpace) * cellSpace;
    }
}