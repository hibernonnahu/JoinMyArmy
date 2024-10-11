using System;
using UnityEngine;


public class StateCharacterEnemyVulnerable : StateCharacterEnemy
{
    public StateCharacterEnemyVulnerable(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy enemy) : base(stateMachine, enemy)
    {
        
    }

    public override void Awake()
    {
        enemy.Rigidbody.drag = 50;
    }
    public override void Sleep()
    {
        enemy.onVulnerableEnd();
        enemy.onVulnerableEnd = () => { };

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
