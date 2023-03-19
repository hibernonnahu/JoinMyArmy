using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyDead : StateCharacterEnemy
{
    public StateCharacterEnemyDead(StateMachine<StateCharacter> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        enemy.SetAnimation("dead",0.1f);
        enemy.DisableCollider();
        enemy.Rigidbody.isKinematic = true;

    }

    public override void Sleep()
    {
        
    }

    public override void Update()
    {
       
    }
    public override void ChangeState(Type type)
    {

    }
}
