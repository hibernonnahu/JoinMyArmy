

using System;
using UnityEngine;

public class CharacterMainAttackController : MonoBehaviour, ICharacterAttackController
{
    private StateMachine<StateAttackHandler> stateMachine;

    public void Init(CharacterMain characterMain, Animator animator, CharacterManager characterManager)
    {
        stateMachine = new StateMachine<StateAttackHandler>();
        stateMachine.AddState(new StateAttackHandlerAlert(stateMachine, characterMain, animator, characterManager));
        stateMachine.AddState(new StateAttackHandlerIdle(stateMachine, characterMain, animator, characterManager));
        stateMachine.AddState(new StateAttackHandlerAttack(stateMachine, characterMain, animator));
    }

    public void GoAttack()
    {
        stateMachine.ChangeState<StateAttackHandlerAttack>();
    }
    public void GoIdle()
    {
        stateMachine.ChangeState<StateAttackHandlerIdle>();
    }
    public void GoAlert()
    {
        stateMachine.ChangeState<StateAttackHandlerAlert>();
    }

    public void GoUpdate()
    {
        stateMachine.Update();
    }
}

