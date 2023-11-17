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
        enemy.SetAnimation("knocked",0.3f);
        enemy.RecluitIconHandler.KnockOut();
        enemy.DisableCollider();
        if (!enemy.IsDead &&!enemy.IsKnocked && enemy.team != enemy.CharacterMain.team)
        {
            enemy.CharacterMain.AddXP(enemy.GetXp());
            enemy.CharacterMain.coinsUIController.AddCoins((int)(enemy.GetCoins() * enemy.CharacterMain.CoinsMultiplier), enemy.transform.position);


            if (enemy.triggerOnDeath != "")
            {
                EventManager.TriggerEvent(enemy.triggerOnDeath);
            }
        }
        enemy.IsKnocked = true;
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {

    }

}
