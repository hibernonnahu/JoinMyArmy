using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyMelee : StateCharacterEnemy
{
    private float counter;
    private bool hit;
    private int maxAttacks;
    public StateCharacterEnemyMelee(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        maxAttacks = UnityEngine.Random.Range(1, 4);
        enemy.animator.SetFloat("attackspeed", enemy.attackSpeed);
        AttackInit();
    }

    private void AttackInit()
    {
        maxAttacks--;
        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.SetAnimation("attack", 0.01f, 0);
        counter = (1 / enemy.attackSpeed) * 0.35f;
        hit = false;
        enemy.model.transform.forward = enemy.lastEnemyTarget.transform.position - enemy.transform.position;
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (!hit)
        {
           enemy.Rigidbody.velocity = Vector3.zero;
            if (counter < 0)
            {
                if ( enemy.lastEnemyTarget.CurrentHealth > 0 &&enemy.team !=enemy.lastEnemyTarget.team && (enemy.transform.position -enemy.lastEnemyTarget.transform.position).sqrMagnitude <enemy.attackDistanceSqr)
                {
                    EventManager.TriggerEvent("playfx", EventManager.Instance.GetEventData().SetString("damage"));
                   enemy.lastEnemyTarget.GetHit(enemy);
                }
                else
                {
                   enemy.lastEnemyTarget = null;
                }
                hit = true;
                counter =enemy.attackSpeed * 0.65f;
            }
        }
        else
        {
            if (counter < 0)
            {
                if (enemy.lastEnemyTarget == null || maxAttacks <= 0)
                {
                    ChangeState(enemy.IdleState);
                }
                else
                {
                    AttackInit();
                }

            }
        }
    }
}
