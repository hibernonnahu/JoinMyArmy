using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyDead : StateCharacterEnemy
{
    public StateCharacterEnemyDead(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        enemy.IdleState = typeof(StateCharacterEnemyDead);
        enemy.SetAnimation("dead", 0.1f);
        enemy.DisableCollider();
        enemy.Rigidbody.isKinematic = true;
        if (!enemy.IsDead&&enemy.team!=enemy.CharacterMain.team)
        {
            enemy.CharacterMain.AddXP(enemy.GetXp());
            enemy.CharacterMain.coinsUIController.AddCoins(enemy.GetCoins(),enemy.transform.position);
           
            foreach (var item in enemy.OnDeadActionList)
            {
                item();
            }
        }
        enemy.IsDead = true;
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        enemy.transform.position -= Vector3.up * Time.deltaTime*0.3f;
    }
    public override void ChangeState(Type type)
    {

    }
}
