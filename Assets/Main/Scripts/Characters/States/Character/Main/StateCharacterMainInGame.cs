using UnityEngine;
using System.Collections;
using System;

public class StateCharacterMainInGame : StateCharacter
{
    protected ICharacterAttackController attackHandler;
    protected CharacterMain characterMain;
    private CameraHandler cameraHandler;
    public StateCharacterMainInGame(StateMachine<StateCharacter> stateMachine, CharacterMain character) : base(stateMachine, character)
    {
        attackHandler = character.GetComponent<ICharacterAttackController>();
        attackHandler.Init(character, character.animator, character.CharacterManager);
        this.characterMain = character;
        characterMain.animator.SetFloat("walkspeed", characterMain.speed);

    }
    public override void Awake()
    {
        character.IdleState = typeof(StateCharacterMainInGame);
        attackHandler.GoAlert();
        cameraHandler = GameObject.FindObjectOfType<CameraHandler>();
        cameraHandler.FollowGameObject(character.model.gameObject, false);
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
            character.SetAnimation("idle",0);
           

            character.Rigidbody.drag = 50;
            character.Rigidbody.velocity = Vector3.zero;
            characterMain.IsMoving = false;
        }
        else
        {
            characterMain.IsMoving = true;
            character.Rigidbody.drag = 0;

            Vector3 direction = CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * y);
            if (direction != Vector3.zero)
            {
                character.Rigidbody.velocity = character.speed * direction;
                if (character.lastEnemyTarget != null)
                {
                    direction = character.lastEnemyTarget.transform.position - character.transform.position;
                }
                character.model.transform.forward = direction;
                character.SetAnimation("walk",0,0);
            }
        }
    }
    public override void Update()
    {
        attackHandler.GoUpdate();
    }

}
