using UnityEngine;
using System.Collections;
using System;

public class StateAttackHandler : State<StateAttackHandler>
{
    protected CharacterMain characterMain;
    protected Animator animator;
    public StateAttackHandler(StateMachine<StateAttackHandler> stateMachine,CharacterMain characterMain, Animator animator) : base(stateMachine)
    {
        this.characterMain = characterMain;
        this.animator = animator;
    }
}
