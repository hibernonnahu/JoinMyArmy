using UnityEngine;
using System.Collections;
using System;

public class StateCharacterEnemyMelee : StateCharacterEnemy
{
    private float counter;
    private bool hit;
    public StateCharacterEnemyMelee(StateMachine<StateCharacter> stateMachine, CharacterEnemy characterEnemy) : base(stateMachine, characterEnemy)
    {

    }
    public override void Awake()
    {
        AttackInit();
    }

    private void AttackInit()
    {
        character.Rigidbody.velocity = Vector3.zero;
        character.SetAnimation("attack", 0.01f, 0);
        counter = character.attackSpeed * 0.35f;
        hit = false;
        character.model.transform.forward = character.lastEnemyTarget.transform.position - character.transform.position;
    }

    public override void Sleep()
    {

    }

    public override void Update()
    {
        counter -= Time.deltaTime;
        if (!hit)
        {
            character.Rigidbody.velocity = Vector3.zero;
            if (counter < 0)
            {
                if (character.lastEnemyTarget.CurrentHealth > 0 && character.team != character.lastEnemyTarget.team && (character.transform.position - character.lastEnemyTarget.transform.position).sqrMagnitude < character.attackDistanceSqr)
                {
                    EventManager.TriggerEvent("playfx", EventManager.Instance.GetEventData().SetString("damage"));
                    character.lastEnemyTarget.GetHit(character);
                }
                else
                {
                    character.lastEnemyTarget = null;
                }
                hit = true;
                counter = character.attackSpeed * 0.65f;
            }
        }
        else
        {
            if (counter < 0)
            {
                if (character.lastEnemyTarget == null)
                {
                    ChangeState(enemy.IdleState);
                }
                else
                {
                    AttackInit();
                }

            }
        }
    }
}
