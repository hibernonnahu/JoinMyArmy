using UnityEngine;
using System.Collections;
using System;

public class StateAttackHandlerAttack : StateAttackHandler
{
    private float counter;
    private bool hit;
    
   
    public StateAttackHandlerAttack(StateMachine<StateAttackHandler> stateMachine, CharacterMain characterMain, Animator animator) : base(stateMachine, characterMain, animator)
    {
    }
    public override void Awake()
    {
        characterMain.animator.SetFloat("attackspeed", characterMain.attackSpeed);

        animator.SetLayerWeight(1, 1);
        characterMain.SetAnimation("attack", 0.01f, 1);

        var main = characterMain.FxHandler.slash.main;
        main.simulationSpeed = characterMain.attackSpeed;
        PlaySlashFx();
        counter = (1/characterMain.attackSpeed) * 0.35f;
        hit = false;
       
        EventManager.TriggerEvent("playfx", EventManager.Instance.GetEventData().SetString("slash"));
    }
    public override void Sleep()
    {
        animator.SetLayerWeight(1, 0);
        
    }
    private void PlaySlashFx()
    {
        characterMain.FxHandler.slash.Play();
    }
    public override void Update()
    {
        counter -= Time.deltaTime;
        if (!hit)
        {
            if (counter < 0)
            {
                if ((characterMain.transform.position - characterMain.lastEnemyTarget.transform.position).sqrMagnitude < characterMain.attackDistanceSqr)
                {
                    characterMain.FxHandler.swordHit.transform.position = characterMain.lastEnemyTarget.transform.position + Vector3.up * 2;
                    characterMain.FxHandler.swordHit.Play();
                    characterMain.lastEnemyTarget.GetHit(characterMain);
                    EventManager.TriggerEvent("playfx", EventManager.Instance.GetEventData().SetString("hit"+ UnityEngine.Random.Range(1,3)));
                }
                hit = true;
               
            }
        }
        else
        {
            if (!characterMain.animator.GetCurrentAnimatorStateInfo(1).IsName("attack"))
            {
                ChangeState(typeof(StateAttackHandlerAlert));
            }
        }
    }
}
