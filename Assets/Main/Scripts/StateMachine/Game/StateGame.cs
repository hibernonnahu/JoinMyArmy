using UnityEngine;
using System.Collections;
using System;

public class StateGame : State<StateGame>
{
    protected Game game;
    public StateGame(StateMachine<StateGame> stateMachine,Game game) :base (stateMachine)
    {
        this.game = game;
    }


    public virtual void Spawn()
    {
       
    }

    public virtual void OnDestroy()
    {
        
    }
}
