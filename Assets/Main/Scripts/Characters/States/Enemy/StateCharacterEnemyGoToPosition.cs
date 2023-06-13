using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyGoToPosition : StateCharacterEnemy
{
    private const float ARRIVE_DIST_SQR = 0.5f;
    public StateCharacterEnemyGoToPosition(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        stearingMask = LayerMask.GetMask(new string[] {  });

        enemy.SetAnimation("walkstory");
        enemy.Rigidbody.drag = 0;
    }

    public override void Sleep()
    {
        enemy.Rigidbody.drag = 100;

    }

    public override void Update()
    {
        Vector3 difVector = enemy.destiny - enemy.transform.position;
        UpdateMovement(difVector.x, difVector.z);
        if ((enemy.transform.position - enemy.destiny).sqrMagnitude < ARRIVE_DIST_SQR)
        {
            enemy.SetAnimation("idle");
            enemy.Rigidbody.velocity = Vector3.zero;
            ChangeState(typeof(StateCharacterEnemyIdle));
            EventManager.TriggerEvent(EventName.STORY_ARRIVE);
        }
    }
}
