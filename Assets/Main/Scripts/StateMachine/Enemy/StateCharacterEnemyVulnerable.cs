using System;
using UnityEngine;


public class StateCharacterEnemyVulnerable : StateCharacterEnemy
{
    public StateCharacterEnemyVulnerable(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy enemy) : base(stateMachine, enemy)
    {
        
    }

    public override void Awake()
    {
        
    }
    public override void UpdateMovement(float x, float y)
    {
        
    }
    public override void Update()
    {
        enemy.VulnerableTime -= Time.deltaTime;
        if (enemy.VulnerableTime<0)
        {
            ChangeState(enemy.NextState);
        }
    }

}
