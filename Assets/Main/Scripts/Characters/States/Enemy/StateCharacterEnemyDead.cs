using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyDead : StateCharacterEnemy
{
    private const float TIME_DISAPEAR = 15;
    private float counter;
    public StateCharacterEnemyDead(StateMachine<StateCharacterEnemy> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        counter = TIME_DISAPEAR;
        enemy.IdleState = typeof(StateCharacterEnemyDead);
        enemy.SetAnimation("dead", 0.1f);
        enemy.DisableCollider();
        enemy.Rigidbody.isKinematic = true;
        enemy.CharacterManager.RemoveCharacter(enemy);

        if (!enemy.IsDead && !enemy.IsKnocked && enemy.team != enemy.CharacterMain.team)
        {
            enemy.CharacterMain.AddXP(enemy.GetXp());
            enemy.CharacterMain.coinsUIController.AddCoins((int)(enemy.GetCoins() * enemy.CharacterMain.CoinsMultiplier), enemy.transform.position);
            if (enemy.triggerOnDeath != "")
            {
                EventManager.TriggerEvent(enemy.triggerOnDeath);
            }
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
        enemy.transform.position -= Vector3.up * Time.deltaTime * 0.2f;
        
        counter -= Time.deltaTime;
        if (counter < 0)
        {
            if (!enemy.extra)
            {
                enemy.gameObject.SetActive(false);
            }
        }
    }
    public override void ChangeState(Type type)
    {

    }

}
