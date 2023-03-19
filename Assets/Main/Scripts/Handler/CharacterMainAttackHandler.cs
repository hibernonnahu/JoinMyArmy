

using System;
using UnityEngine;

public class CharacterMainAttackHandler
{
    private StateMachine<StateAttackHandler> stateMachine;
   
    public CharacterMainAttackHandler(CharacterMain characterMain,Animator animator, CharacterManager characterManager)
    {
        stateMachine = new StateMachine<StateAttackHandler>();
        stateMachine.AddState(new StateAttackHandlerAlert(stateMachine,characterMain,animator,characterManager));
        stateMachine.AddState(new StateAttackHandlerIdle(stateMachine,characterMain,animator,characterManager));
        stateMachine.AddState(new StateAttackHandlerAttack(stateMachine, characterMain,animator));
    }

    internal void GoIdle()
    {
        stateMachine.ChangeState<StateAttackHandlerIdle>();
    }
    internal void GoAlert()
    {
        stateMachine.ChangeState<StateAttackHandlerAlert>();
    }

    internal void Update()
    {
        stateMachine.Update();
    }
}

