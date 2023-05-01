using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyChase : StateCharacterEnemy
{
    private const float TICK_TIME = 0.2f;
    protected float counter;

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
                UpdateMovement(difVector.x, difVector.z);
            }
        }
    }
    internal override void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 8)
        {
            var colCharacter = collision.gameObject.GetComponent<Character>();
            if (colCharacter && colCharacter.team != enemy.team)
                if (colCharacter == enemy.lastEnemyTarget)
                {
                    counter = -1;
                }
                else
                {
                    if ((colCharacter.transform.position - enemy.transform.position).sqrMagnitude <
                        (enemy.lastEnemyTarget.transform.position - enemy.transform.position).sqrMagnitude)
                    {
                        enemy.lastEnemyTarget = colCharacter;
                    }
                }
        }
    }
}
