using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateGameChapterFinish : StateGame
{
    private string music;
    private bool win;
    private float delay;
    public StateGameChapterFinish(StateMachine<StateGame> stateMachine, Game game, bool win, string music, float delay = 0) : base(stateMachine, game)
    {
        this.win = win;
        this.music = music;
        this.delay = delay;
    }


    public override void Awake()
    {

        LeanTween.color(game.fadeImage.rectTransform, Color.black, 2f).setDelay(delay).setOnComplete(DelayEnd);
        var mm = GameObject.FindObjectOfType<MusicManager>();

        if (mm!=null)
        {
            mm.SetCurrentAsDefault();
            mm.LoadClip(music);
            mm.PlayMusic(music, 1, false);
        }
       
        //add fanfare

    }

    public override void Update()
    {

    }


    public override void Sleep()
    {
    }
    void DelayEnd()
    {
        LeanTween.delayedCall(1.4f, End).setIgnoreTimeScale(true);
    }
    void End()
    {
        EventManager.TriggerEvent(EventName.MAIN_TEXT, EventManager.Instance.GetEventData().SetString(""));
        game.coinsCollectUIController.StartAnimation(win);
        //EventManager.TriggerEvent(EventName.POPUP_OPEN, EventManager.Instance.GetEventData().SetString(PopupName.WIN));
        ChangeState(typeof(StateGame));
    }
}