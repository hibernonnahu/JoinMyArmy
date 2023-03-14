using UnityEngine;
using System.Collections;
using System;

public class StateCharacterMainInGame : StateCharacter
{
    public StateCharacterMainInGame(StateMachine<StateCharacter> stateMachine, Character character) : base(stateMachine, character)
    {

    }

    public override void UpdateMovement(float x, float y)
    {
        character.Rigidbody.velocity = character.speed * Utils.Normalize(Vector3.right * x + Vector3.forward * y);
    }

}
