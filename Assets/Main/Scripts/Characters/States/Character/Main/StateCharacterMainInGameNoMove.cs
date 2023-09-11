using UnityEngine;
using System.Collections;
using System;

public class StateCharacterMainInGameNoMove: StateCharacterMainInGame 
{
   
    public StateCharacterMainInGameNoMove(StateMachine<StateCharacter> stateMachine, CharacterMain character) : base(stateMachine, character)
    {
       

    }

    public override void Awake()
    {
        Debug.Log("");
    }
    public override void UpdateMovement(float x, float y)
    {
      
            character.SetAnimation("idle",0);
            character.Rigidbody.drag = 50;
            character.Rigidbody.velocity = Vector3.zero;
            characterMain.IsMoving = false;
       
    }
    

}
