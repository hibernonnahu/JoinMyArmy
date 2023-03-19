using UnityEngine;
using System.Collections;
using System;

public class StateCharacter : State<StateCharacter>
{
    protected Character character;
    public StateCharacter(StateMachine<StateCharacter> stateMachine, Character character) : base(stateMachine)
    {
        this.character = character;
    }

    public virtual void UpdateMovement(float x, float y) { }
    //public virtual void Spawn() { }
    //public virtual void OnDestroy() { }
    public virtual void GetHit(float damage)
    {
        character.CurrentHealth -= damage;
        if (character.CurrentHealth <= 0)
        {
            character.CurrentHealth = 0;
            ChangeState(typeof(StateCharacterMainDead));
        }
        character.HealthBarHandler.UpdateBar();
    }

}
