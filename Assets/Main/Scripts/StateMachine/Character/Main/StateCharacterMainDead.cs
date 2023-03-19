using System;
using UnityEngine;


public class StateCharacterMainDead : StateCharacter
{
    public StateCharacterMainDead(StateMachine<StateCharacter> stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void Awake()
    {
        character.Rigidbody.velocity = Vector3.zero;
        character.CharacterManager.RemoveCharacter(character);
        character.SetAnimation("dead");
    }
    public override void ChangeState(Type type)
    {
        
    }
    public override void GetHit(float damage)
    {
        
    }
}
