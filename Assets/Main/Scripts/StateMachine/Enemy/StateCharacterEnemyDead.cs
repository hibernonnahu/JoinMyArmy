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
        enemy.model.transform.localScale = Vector3.one * 0.1f;
    }

    public override void Sleep()
    {
        
    }

    public override void Update()
    {
       
    }
}
