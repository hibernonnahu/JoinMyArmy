using System;
using UnityEngine;


public class StateCharacterMainIdle : StateCharacter
{
    CharacterMain characterMain;
    public StateCharacterMainIdle(StateMachine<StateCharacter> stateMachine, CharacterMain character) : base(stateMachine, character)
    {
        characterMain = character;
    }

    public override void Awake()
    {
        character.SetAnimation("idle");
        character.Rigidbody.velocity = Vector3.zero;
        characterMain.floatingJoystick.OnPointerUp(null);
    }
   
    public override bool GetHit(float damage, Character attacker)
    {
        return false;
    }
    
}
