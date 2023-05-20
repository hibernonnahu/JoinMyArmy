using System;
using UnityEngine;


public class StateCharacterVulnerable : StateCharacter
{
    public StateCharacterVulnerable(StateMachine<StateCharacter> stateMachine, Character character) : base(stateMachine, character)
    {
        
    }

    public override void Awake()
    {
        character.Rigidbody.drag = 50;
    }
    public override void UpdateMovement(float x, float y)
    {
        
    }
    public override void Update()
    {
        character.VulnerableTime -= Time.deltaTime;
        if (character.VulnerableTime<0)
        {
            ChangeState(character.NextState);
        }
    }

}
