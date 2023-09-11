using UnityEngine;
using System.Collections;
using System;

public class StateAttackSimpleHandlerAttack : StateAttackHandler
{
    private float counter;
    private bool hit;
  

    public StateAttackSimpleHandlerAttack(StateMachine<StateAttackHandler> stateMachine, CharacterMain characterMain, Animator animator) : base(stateMachine, characterMain, animator)
    {
       
    }
    public override void Awake()
    {
        characterMain.animator.SetFloat("attackspeed", characterMain.AttackSpeed);

        characterMain.SetAnimation("attack", 0);

        counter = (1 / characterMain.AttackSpeed) * 0.35f;
        hit = false;
        characterMain.Rigidbody.velocity = Vector3.zero;
        characterMain.StateMachine.CurrentState.ChangeState(typeof(StateCharacterMainInGameNoMove));
    }
    public override void Sleep()
    {
        characterMain.StateMachine.CurrentState.ChangeState(typeof(StateCharacterMainInGame));
    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (!hit)
        {
            if (counter < 0)
            {
               
                    characterMain.lastEnemyTarget.GetHit(characterMain);
                
                hit = true;

            }
        }
        else
        {
            if (!characterMain.animator.GetCurrentAnimatorStateInfo(0).IsName("attack"))
            {
                ChangeState(typeof(StateAttackHandlerAlert));
            }
        }
    }
    public override void ChangeState(Type type)
    {
        if(type!=typeof(StateAttackHandlerIdle))
        base.ChangeState(type);
    }
}
