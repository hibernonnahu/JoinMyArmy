using System;
using UnityEngine;


public class StateCharacterMainDead : StateCharacter
{
    private CharacterMain characterMain;
    public StateCharacterMainDead(StateMachine<StateCharacter> stateMachine, CharacterMain character) : base(stateMachine, character)
    {
        this.characterMain = character;
    }

    public override void Awake()
    {
        character.IdleState = typeof(StateCharacterMainDead);
        character.Rigidbody.velocity = Vector3.zero;
        character.CharacterManager.RemoveCharacter(character);
        character.SetAnimation("dead");
        characterMain.IsDead = true;
        characterMain.lastEnemyTarget = null;
    }
    public override void ChangeState(Type type)
    {

    }
    public override void GetHit(float damage)
    {

    }
}
