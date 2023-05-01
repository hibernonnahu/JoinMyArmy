using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyAlert : StateCharacterEnemy
{
    private const float TICK_TIME = 1;
    private float counter;
    private float alertDistanceSqr;
    private EnemyStateAttackModeController enemyStateAttackModeHandler;
    public StateCharacterEnemyAlert(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy,EnemyStateAttackModeController enemyStateAttackModeHandler,float alertDistanceSqr) : base(stateMachine, characterEnemy)
    {
        this.enemyStateAttackModeHandler = enemyStateAttackModeHandler;
        enemy.animator.SetFloat("walkspeed", enemy.speed);
        this.alertDistanceSqr = alertDistanceSqr;
    }
    public override void Awake()
    {
        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.SetAnimation("idle", 0.02f);
        counter = 0;
    }

    public override void Sleep()
    {
        
    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            enemy.lastEnemyTarget = enemy.CharacterManager.GetClosestEnemyInRange(enemy.team, alertDistanceSqr + enemy.extraAlertRange, enemy.model.transform.position);
            if (enemy.lastEnemyTarget == null)
            {
                counter = TICK_TIME;
                if(UnityEngine.Random.value<0.2f)
                ChangeState(typeof(StateCharacterEnemyGoBack));
            }
            else
            {
                enemyStateAttackModeHandler.Attack();
                
            }
        }
    }
}
