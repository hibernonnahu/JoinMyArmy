using UnityEngine;
using System.Collections;
using System;

public class StateGame : State<StateGame>
{
    public StateGame(StateMachine<StateGame> stateMachine) :base (stateMachine)
    {
       
    }


    public virtual void Spawn()
    {
       
    }

    public virtual void OnDestroy()
    {
        
    }
}
