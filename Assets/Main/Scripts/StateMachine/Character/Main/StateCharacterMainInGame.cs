using UnityEngine;
using System.Collections;
using System;

public class StateCharacterMainInGame : StateCharacter
{
    private CharacterMainAttackController attackHandler;
    private CharacterMain characterMain;
    public StateCharacterMainInGame(StateMachine<StateCharacter> stateMachine, CharacterMain character) : base(stateMachine, character)
    {
        attackHandler = new CharacterMainAttackController(character, character.animator,character.CharacterManager);
        this.characterMain = character;
        characterMain.animator.SetFloat("walkspeed", characterMain.speed);

    }
    public override void Awake()
    {
        character.IdleState = typeof(StateCharacterMainInGame);
        attackHandler.GoAlert();
    }
    public override void Sleep()
    {
        attackHandler.GoIdle();
        characterMain.IsMoving = false;
    }
    public override void UpdateMovement(float x, float y)
    {
        if (x ==0&&y==0)
        {
            character.SetAnimation("idle",0.02f);
            character.Rigidbody.velocity = Vector3.zero;
            characterMain.IsMoving = false;
        }
        else
        {
            characterMain.IsMoving = true;

            Vector3 direction = Utils.Normalize(Vector3.right * x + Vector3.forward * y);
            if (direction != Vector3.zero)
            {
                character.Rigidbody.velocity = character.speed * direction;
                if (character.lastEnemyTarget != null)
                {
                    direction = character.lastEnemyTarget.transform.position - character.transform.position;
                }
                character.model.transform.forward = direction;
                character.SetAnimation("walk");
            }
        }
    }
    public override void Update()
    {
        attackHandler.Update();
    }

}
