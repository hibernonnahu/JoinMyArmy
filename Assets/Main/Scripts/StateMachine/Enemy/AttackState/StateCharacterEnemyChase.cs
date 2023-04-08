using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyChase : StateCharacterEnemy
{
    private const float TICK_TIME=1;
    private float counter;
    public StateCharacterEnemyChase(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        counter = 0;
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
            Vector3 difVector = enemy.lastEnemyTarget.transform.position - enemy.transform.position;
            if (difVector.sqrMagnitude < enemy.attackDistanceSqr)
            {
                ChangeState(enemy.NextState);
            }
            else
            {
                enemy.model.transform.forward = CustomMath.XZNormalize(difVector);
                enemy.Rigidbody.velocity = enemy.model.transform.forward * enemy.speed;
            }
        }
    }
}
