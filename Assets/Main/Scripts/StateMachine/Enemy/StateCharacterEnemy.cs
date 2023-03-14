using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemy : StateCharacter
{
    protected CharacterEnemy enemy;
    public StateCharacterEnemy(StateMachine<StateCharacter> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {
        this.enemy = characterEnemy;
    }

    public override void UpdateMovement(float x, float y)
    {
        character.Rigidbody.velocity = character.speed * CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * y);
    }
    public override void GetHit(float damage)
    {
        enemy.CurrentHealth -= damage;
        if (enemy.CurrentHealth <= 0)
        {
            enemy.CurrentHealth = 0;
            ChangeState(typeof(StateCharacterEnemyKnocked));
        }
        enemy.UpdateBar();
    }
}
