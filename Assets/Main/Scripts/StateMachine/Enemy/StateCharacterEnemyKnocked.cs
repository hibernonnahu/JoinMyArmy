using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyKnocked : StateCharacterEnemy
{
    public StateCharacterEnemyKnocked(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        enemy.IdleState = typeof(StateCharacterEnemyKnocked);
        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.CharacterManager.RemoveCharacter(enemy);
        enemy.SetAnimation("knocked",0.1f);
        enemy.RecluitIconHandler.KnockOut();
        enemy.DisableCollider();
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {

    }
}
