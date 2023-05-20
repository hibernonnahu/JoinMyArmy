using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateInGame : StateGame
{
    private List<Action> extraActions = new List<Action>();
    private float waveTimeCounter;
   


    public StateInGame(StateMachine<StateGame> stateMachine, Game game) : base(stateMachine, game)
    {
        CheckExtraActions(game);
    }

   
    public override void Awake()
    {

    }

    public override void Update()
    {
        foreach (var item in extraActions)
        {
            item();
        }
    }


    public override void Sleep()
    {
    }
    private void CheckExtraActions(Game game)
    {
        if (game.time > 0)
        {
            extraActions.Add(OnTimeLeft);
        }
        if (game.CharacterManager.HasSpawnEnemies())
        {
            EventManager.StartListening(EventName.SPAWN_WAVE, OnSpawnWave);
            if (game.CharacterManager.GetTeam(1).Count == 0)
            {
                waveTimeCounter = game.waveTime;
                extraActions.Add(OnWaveTimeLeft);
            }
        }
    }

    private void OnSpawnWave(EventData arg0)
    {
        if (!extraActions.Contains(OnWaveTimeLeft))
        {
            extraActions.Add(OnWaveTimeLeft);
        }
        waveTimeCounter = game.waveTime;
    }

    private void OnTimeLeft()
    {
        game.time -= Time.deltaTime;
        game.gameUIController.UpdateTimeLeft(Mathf.FloorToInt(game.time));
       
    }
    private void OnWaveTimeLeft()
    {
        waveTimeCounter -= Time.deltaTime;
        game.gameUIController.UpdateWaveTimeLeft(Mathf.FloorToInt(waveTimeCounter),game.CharacterManager);
    }
}