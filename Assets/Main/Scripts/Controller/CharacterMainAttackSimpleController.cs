

using UnityEngine;

public class CharacterMainAttackSimpleController : MonoBehaviour, ICharacterAttackController
{
    private StateMachine<StateAttackHandler> stateMachine;
    bool init = false;
   
    public void Init(CharacterMain characterMain,Animator animator, CharacterManager characterManager)
    {
        if (!init)
        {
            init = true;
            stateMachine = new StateMachine<StateAttackHandler>();
            stateMachine.AddState(new StateAttackHandlerAlert(stateMachine, characterMain, animator, characterManager));
            stateMachine.AddState(new StateAttackHandlerIdle(stateMachine, characterMain, animator, characterManager));
            stateMachine.AddState(new StateAttackSimpleHandlerAttack(stateMachine, characterMain, animator));
            characterMain.StateMachine.AddState(new StateCharacterMainInGameNoMove(characterMain.StateMachine, characterMain));
        }
    }

    public void GoAttack()
    {
        stateMachine.ChangeState<StateAttackSimpleHandlerAttack>();
    }
    public void GoIdle()
    {
        stateMachine.CurrentState.ChangeState(typeof(StateAttackHandlerIdle));
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

