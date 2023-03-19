using UnityEngine;
using System.Collections;
using System;

public class StateAttackHandlerIdle : StateAttackHandler
{
    private const float TICK_TIME = 0.5f;
    private CharacterManager characterManager;
    private float tick;
    private CrossHairHandler crossHairHandler;
    public StateAttackHandlerIdle(StateMachine<StateAttackHandler> stateMachine, CharacterMain characterMain, Animator animator, CharacterManager characterManager) :
        base(stateMachine, characterMain, animator)
    {
        this.characterManager = characterManager;
        crossHairHandler = GameObject.FindObjectOfType<CrossHairHandler>();
    }

    public override void Awake()
    {
        crossHairHandler.UnFollow();
        animator.SetLayerWeight(1, 0);

    }
   
}
