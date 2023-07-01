using UnityEngine;
using System.Collections;
using System;

public class StateCharacterMainGoToPosition : StateCharacter
{
    private const float ARRIVE_DIST_SQR = 0.5f;
    private int stearingMask;
    public StateCharacterMainGoToPosition(StateMachine<StateCharacter> stateMachine, CharacterMain character) : base(stateMachine, character)
    {
        stearingMask = LayerMask.GetMask(new string[] { "Wall", "Water" });

    }
    public override void Awake()
    {

        character.SetAnimation("walkstory");
        character.Rigidbody.drag = 0;
    }

    public override void Sleep()
    {
        character.Rigidbody.drag = 100;
    }

    public override void Update()
    {
        Vector3 difVector = character.destiny - character.transform.position;
        UpdateMovement(difVector.x, difVector.z);
        if ((character.transform.position - character.destiny).sqrMagnitude < ARRIVE_DIST_SQR)
        {
            character.SetAnimation("idle");
            character.Rigidbody.velocity = Vector3.zero;
           
            EventManager.TriggerEvent(EventName.STORY_ARRIVE);
            character.VulnerableTime = 999;
            ChangeState(typeof(StateCharacterVulnerable));
        }
    }
    public override void UpdateMovement(float x, float y)
    {
        Vector3 forward = Vector3.Slerp(character.model.transform.forward, CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * y), 43 * Time.deltaTime);
        Vector3 stearing = Utils.StearingVector(character.transform.position + Vector3.up * 2, forward, stearingMask);

        if (stearing != Vector3.zero)
        {

            stearing = CustomMath.XZNormalize((stearing));
            // Debug.DrawRay(enemy.transform.position, stearing *3, Color.magenta, 1);
            character.model.transform.forward = stearing;
        }
        else
        {

            character.model.transform.forward = forward;

        }


        character.Rigidbody.velocity = character.speed * character.model.transform.forward;
    }
    //public override void UpdateMovement(float x, float y)
    //{
    //    Vector3 direction = CustomMath.XZNormalize(Vector3.right * x + Vector3.forward * y);
    //    if (direction != Vector3.zero)
    //    {
    //        character.model.transform.forward = direction;
    //        character.Rigidbody.velocity = character.speed * direction;
    //    }

    //}
}
