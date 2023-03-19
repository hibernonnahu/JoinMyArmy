using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyKnocked : StateCharacterEnemy
{
    public StateCharacterEnemyKnocked(StateMachine<StateCharacter> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        enemy.Rigidbody.velocity = Vector3.zero;
        enemy.CharacterManager.RemoveCharacter(enemy);
        enemy.SetAnimation("knocked");
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
