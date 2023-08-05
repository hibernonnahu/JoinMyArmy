using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyHelpAttack : StateCharacterEnemy
{
    private const float TICK_TIME = 1;
    private float counter;
    public StateCharacterEnemyHelpAttack(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        counter = 0;
        enemy.Rigidbody.drag = 0;
        enemy.SetAnimation("walk");
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            counter = TICK_TIME;
            if (enemy.CharacterMain.lastEnemyTarget == null || enemy.CharacterMain.lastEnemyTarget.IsDead || enemy.CharacterMain.lastEnemyTarget.IsKnocked)
            {
                ChangeState(typeof(StateCharacterEnemyFollowLeader));
            }
            else
            {
                Vector3 difVector = enemy.CharacterMain.lastEnemyTarget.transform.position - enemy.transform.position;
                if (difVector.sqrMagnitude < enemy.attackDistanceSqr)
                {
                    enemy.lastEnemyTarget = enemy.CharacterMain.lastEnemyTarget;
                    ChangeState(enemy.AttackState);
                }
                else
                {
                    enemy.model.transform.forward = CustomMath.XZNormalize(difVector);
                    enemy.Rigidbody.velocity = enemy.model.transform.forward * enemy.speed;
                }
            }
        }
    }
}
