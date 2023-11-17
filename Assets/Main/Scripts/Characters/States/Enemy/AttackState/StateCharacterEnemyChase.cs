using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyChase : StateCharacterEnemy
{
    private const float TICK_TIME = 0.2f;
    private const int LOOP_PER_CHECK = 5;
    private float forceIdleAfterSeconds;
    private float forceIdleAfterSecondsCounter;
    protected float counter;
    protected float alertRange;
    private int loops;
    public StateCharacterEnemyChase(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy, float alertRange, float forceIdleAfterSeconds) : base(stateMachine, characterEnemy)
    {
        this.alertRange = alertRange;
        this.forceIdleAfterSeconds = forceIdleAfterSeconds;
    }
    public override void Awake()
    {
        counter = 0;
        if (!enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("walk"))
        {
            enemy.SetAnimation("walk");
            enemy.animator.SetFloat("walkspeed", enemy.speed);
        }
        enemy.Rigidbody.drag = 0;
        loops = 0;
        forceIdleAfterSecondsCounter = forceIdleAfterSeconds;
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            forceIdleAfterSecondsCounter -= counter;

            loops++;
            counter = TICK_TIME;
            if (loops == LOOP_PER_CHECK)
            {
                enemy.lastEnemyTarget = enemy.CharacterManager.GetClosestEnemyInRange(enemy.team, alertRange, enemy.transform.position);
                if (enemy.lastEnemyTarget == null || forceIdleAfterSecondsCounter < 0)
                {
                    ChangeState(enemy.IdleState);
                    return;
                }
            }
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
    public override bool GetHit(float damage, Character attacker)
    {
        if (attacker != null && (attacker.transform.position - enemy.transform.position).sqrMagnitude < (enemy.lastEnemyTarget.transform.position - enemy.transform.position).sqrMagnitude)
        {
            enemy.lastEnemyTarget = attacker;
        }
        return base.GetHit(damage, attacker);

    }
    internal override void OnCollisionEnter(Collision collision)
    {
        if (enemy.HitsLayer(collision.gameObject.layer))
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
