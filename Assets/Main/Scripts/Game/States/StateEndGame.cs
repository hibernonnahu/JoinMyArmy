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
        
        EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(true));

        stats.level++;
        var next = Resources.LoadAll<TextAsset>("Maps/Campaign/Book" + stats.book + "/Chapter" + stats.chapter + "/Level" + stats.level);
        if (next == null || next.Length == 0)
        {
            game.coinsCollectUIController.gameObject.SetActive(true);
            SaveData.GetInstance().SaveRam(false);
            EventManager.TriggerEvent(EventName.MAIN_TEXT, EventManager.Instance.GetEventData().SetString("Chapter "+stats.chapter+" complete!"));
            if (stats.chapter < 4)
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
            ChangeState(typeof(StateGameChapterFinish));

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
        EventManager.TriggerEvent(EventName.HIDE_CHARACTER_UI, EventManager.Instance.GetEventData().SetBool(true));
        var coinsController = GameObject.FindObjectOfType<CoinsUIController>();
        if (coinsController != null)
        {
            coinsController.gameObject.SetActive(false);
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