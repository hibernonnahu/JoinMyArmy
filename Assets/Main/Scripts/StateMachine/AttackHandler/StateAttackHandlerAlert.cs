using UnityEngine;
using System.Collections;
using System;

public class StateAttackHandlerAlert : StateAttackHandler
{
    private const float TICK_TIME = 0.5f;
    private CharacterManager characterManager;
    private float tick;
    private CrossHairHandler crossHairHandler;
    public StateAttackHandlerAlert(StateMachine<StateAttackHandler> stateMachine, CharacterMain characterMain, Animator animator, CharacterManager characterManager) :
        base(stateMachine, characterMain, animator)
    {
        this.characterManager = characterManager;
        crossHairHandler = GameObject.FindObjectOfType<CrossHairHandler>();
    }

    public override void Awake()
    {
        tick = 0;

    }
    public override void Update()
    {
        tick -= Time.deltaTime;
        if (tick < 0)
        {
            tick = TICK_TIME;
            if (characterMain.lastEnemyTarget == null || characterMain.lastEnemyTarget.CurrentHealth <= 0 ||
                ((characterMain.transform.position - characterMain.lastEnemyTarget.transform.position).sqrMagnitude) > characterMain.attackDistanceSqr)
            {
                crossHairHandler.UnFollow();
                characterMain.lastEnemyTarget = characterManager.GetClosestEnemyInRange(characterMain.team, characterMain.attackDistanceSqr, characterMain.transform.position);
                if (characterMain.lastEnemyTarget != null)
                {
                    characterMain.model.transform.forward = characterMain.lastEnemyTarget.transform.position - characterMain.transform.position;
                    crossHairHandler.FollowEnemy(characterMain.lastEnemyTarget.model);
                    ChangeState(typeof(StateAttackHandlerAttack));
                }

            }
            else
            {
                ChangeState(typeof(StateAttackHandlerAttack));
            }
        }
    }
}
