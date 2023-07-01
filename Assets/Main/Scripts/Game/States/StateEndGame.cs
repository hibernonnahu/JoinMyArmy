using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class StateEndGame : StateGame
{

    private CharacterMain characterMain;

    public StateEndGame(StateMachine<StateGame> stateMachine, Game game,CharacterMain characterMain) : base(stateMachine, game)
    {
        this.characterMain = characterMain;
    }

   
    public override void Awake()
    {
        var stats = CurrentPlaySingleton.GetInstance();
        stats.level++;
        var next = Resources.LoadAll<TextAsset>("Maps/Campaign/Book" + stats.book + "/Chapter" + stats.chapter + "/Level" + stats.level);
        if (next == null || next.Length == 0)
        {
            SaveData.GetInstance().SaveRam();
            if (stats.chapter < 3)
            {
                stats.chapter++;
                int currentChapter = SaveData.GetInstance().GetValue(SaveDataKey.CURRENT_CHAPTER, 1);
                if (stats.chapter > currentChapter)
                {
                    CurrentPlaySingleton.GetInstance().animateTransition = true;
                    SaveData.GetInstance().Save(SaveDataKey.CURRENT_CHAPTER, stats.chapter);
                }
            }
            else
            {
                stats.chapter = 1;
            }
            stats.SaveGamePlay(characterMain);
            stats.Reset();
            EventManager.TriggerEvent(EventName.POPUP_OPEN, EventManager.Instance.GetEventData().SetString(PopupName.WIN));
        }
        else
        {
            foreach (var item in GameObject.FindObjectsOfType<RecluitIconController>())
            {
                item.ForceKnocked();
            }

            stats.SaveGamePlay(characterMain);

            LeanTween.color(game.fadeImage.rectTransform, Color.black, 2.5f).setOnComplete(NextLevelScene);
        }
    }

    public override void Update()
    {
       
    }


    public override void Sleep()
    {
    }
    private void NextLevelScene()
    {
        SceneManager.LoadScene("Game");
    }


}