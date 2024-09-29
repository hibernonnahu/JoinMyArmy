using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class StateEndCastleDefense : StateGame
{

    public StateEndCastleDefense(StateMachine<StateGame> stateMachine, Game game) : base(stateMachine, game)
    {

    }


    public override void Awake()
    {
        var stats = CurrentPlaySingleton.GetInstance();
        EventManagerGlobal.TriggerEvent(QudoCustomEvents.ACHIEVEMENT, EventManagerGlobal.Instance.GetEventData().SetString("defense" + stats.chapter).SetString2(""));

        EventManager.TriggerEvent(EventName.HIDE_TEXT, EventManager.Instance.GetEventData().SetBool(true));

        string levelKey = SaveDataKey.CASTLE_DEFENSE_BOOK_CHAPTER_LEVEL_WIN + "_" + stats.book + "_" + stats.chapter;
        string maxCastleKey = SaveDataKey.CASTLE_DEFENSE_BOOK_CHAPTER_LEVEL_WIN + "_" + stats.book;
        
        SaveData.GetInstance().Save(levelKey, SaveData.GetInstance().GetValue(levelKey, 0) + 1);

        game.coinsCollectUIController.gameObject.SetActive(true);
        SaveData.GetInstance().SaveRam(false);
        EventManager.TriggerEvent(EventName.MAIN_TEXT, EventManager.Instance.GetEventData().SetString("Combat Defense success!"));
        int maxCastle = SaveData.GetInstance().GetValue(maxCastleKey, 1);
        if (stats.chapter == maxCastle)
        {
            maxCastle++;
            SaveData.GetInstance().Save(maxCastleKey, maxCastle);
            if (maxCastle <= SaveData.GetInstance().GetValue(SaveDataKey.CURRENT_BOOK_CHAPTER + stats.book, CurrentPlaySingleton.GetInstance().initialChapter))
                CurrentPlaySingleton.GetInstance().animateTransition = true;
        }

        ChangeState(typeof(StateGameChapterFinish));

        var sql = GameObject.FindObjectOfType<SQLManager>();
        if (sql != null)
            sql.SaveUser();
        EventManager.TriggerEvent(EventName.HIDE_CHARACTER_UI, EventManager.Instance.GetEventData().SetBool(true));
        var coinsController = GameObject.FindObjectOfType<CoinsUIController>();
        if (coinsController != null)
        {
            coinsController.gameObject.SetActive(false);
        }
        stats.Reset();

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