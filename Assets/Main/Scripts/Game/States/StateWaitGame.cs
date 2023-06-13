using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class StateWaitGame : StateGame
{
    private float counter=0.5f;
    public StateWaitGame(StateMachine<StateGame> stateMachine, Game game) : base(stateMachine, game)
    {
        
    }

   
    public override void Awake()
    {
       
    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            ChangeState(typeof(StateInGame));
        }
    }

    public override void Sleep()
    {
    }
   
}