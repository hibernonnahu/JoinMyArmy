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
        enemy.SetAnimation("knocked",0.4f);
        enemy.RecluitIconHandler.KnockOut();
        enemy.DisableCollider();
        if (!enemy.IsDead && enemy.team != enemy.CharacterMain.team)
        {
            enemy.CharacterMain.AddXP(enemy.GetXp());
            enemy.CharacterMain.coinsUIController.AddCoins(enemy.GetCoins(), enemy.transform.position);

            enemy.IsDead = true;
        }
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {

    }
}
