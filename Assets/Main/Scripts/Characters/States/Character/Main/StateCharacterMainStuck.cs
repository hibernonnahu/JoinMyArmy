using System;
using UnityEngine;


public class StateCharacterMainStuck : StateCharacter
{
    CharacterMain characterMain;
    public StateCharacterMainStuck(StateMachine<StateCharacter> stateMachine, CharacterMain character) : base(stateMachine, character)
    {
        characterMain = character;
    }

    public override void Awake()
    {
        
        character.Rigidbody.velocity = Vector3.zero;
        characterMain.floatingJoystick.OnPointerUp(null);
    }
    public override void Update()
    {
        characterMain.VulnerableTime -= Time.deltaTime;
        if (characterMain.VulnerableTime < 0)
        {
            ChangeState(character.IdleState);
        }
    }
   
    
}
