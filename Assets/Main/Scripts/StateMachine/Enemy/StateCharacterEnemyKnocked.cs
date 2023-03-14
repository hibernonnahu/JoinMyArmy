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
        enemy.recluitIconHandler.KnockOut();
    }

    public override void Sleep()
    {
        
    }

    public override void Update()
    {
       
    }
}
