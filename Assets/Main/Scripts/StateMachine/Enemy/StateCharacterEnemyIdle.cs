using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyIdle : StateCharacterEnemy
{
    public StateCharacterEnemyIdle(StateMachine<StateCharacter> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        
    }

    public override void Sleep()
    {
        
    }

    float tick = 1;
    public override void Update()
    {
        tick -= Time.deltaTime;
        if (tick < 0)
        {
            GetHit(40.2f);
            tick = 1;
        }
    }
}
